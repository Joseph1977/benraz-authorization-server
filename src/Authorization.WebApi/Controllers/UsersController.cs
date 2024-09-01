using Authorization.Domain;
using Authorization.Domain.Users;
using Authorization.EF.Extensions;
using Authorization.WebApi.Authorization;
using Authorization.WebApi.Filtes;
using Authorization.WebApi.Models.Users;
using Authorization.WebApi.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoreLinq;
using Benraz.Infrastructure.Common.AccessControl;
using Benraz.Infrastructure.Common.Paging;
using Benraz.Infrastructure.Domain.Authorization;
using Benraz.Infrastructure.Web.Authorization;
using Benraz.Infrastructure.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using IAuthorizationService = Microsoft.AspNetCore.Authorization.IAuthorizationService;

namespace Authorization.WebApi.Controllers
{
    /// <summary>
    /// Users controller.
    /// </summary>
    [ApiController]
    [ServiceFilter(typeof(LogUsageFilter))]
    [Route("/v{version:ApiVersion}/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUsersRepository _usersRepository;
        private readonly IUsersService _usersService;
        private readonly IUserPasswordsRepository _userPasswordsRepository;
        private readonly IUserPasswordsService _userPasswordsService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMaskingDataService _maskingDataService;
        private readonly IPrincipalService _principalService;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;

        /// <summary>
        /// Creates controller.
        /// </summary>
        /// <param name="userManager">User manager.</param>
        /// <param name="userStore">User store.</param>
        /// <param name="usersRepository">Users repository.</param>
        /// <param name="usersService">Users service.</param>
        /// <param name="userPasswordsRepository">User passwords repository.</param>
        /// <param name="userPasswordsService">User passwords service.</param>
        /// <param name="authorizationService">Authorization service.</param>
        /// <param name="maskingDataService">Masking data service.</param>
        /// <param name="principalService">Principal service.</param>
        /// <param name="mapper">Auto mapper.</param>
        /// <param name="logger">Logger.</param>
        public UsersController(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            IUsersRepository usersRepository,
            IUsersService usersService,
            IUserPasswordsRepository userPasswordsRepository,
            IUserPasswordsService userPasswordsService,
            IAuthorizationService authorizationService,
            IMaskingDataService maskingDataService,
            IPrincipalService principalService,
            IMapper mapper,
            ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _userStore = userStore;
            _usersRepository = usersRepository;
            _usersService = usersService;
            _userPasswordsRepository = userPasswordsRepository;
            _userPasswordsService = userPasswordsService;
            _authorizationService = authorizationService;
            _maskingDataService = maskingDataService;
            _principalService = principalService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Returns users page.
        /// </summary>
        /// <param name="query">Query.</param>
        /// <returns>Users page.</returns>
        [HttpGet]
        [Authorize(ApplicationPolicies.USER_READ)]
        [ProducesResponseType(typeof(Page<UserInfoViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsersAsync([FromQuery] UsersQuery query)
        {
            var usersPage = await _usersRepository.GetPageAsync(query);

            var viewModelsPage = _mapper.Map<Page<UserInfoViewModel>>(usersPage);
            viewModelsPage.Items.ForEach(x => AdjustMasking(x));

            return Ok(viewModelsPage);
        }

        /// <summary>
        /// Returns user by identifier.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>User.</returns>
        [HttpGet("{userId}")]
        [Authorize(ApplicationPolicies.USER_READ_ONE)]
        [ProducesResponseType(typeof(UserInfoViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserAsync([FromRoute] string userId)
        {
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var viewModel = _mapper.Map<UserInfoViewModel>(user);
            AdjustMasking(viewModel);

            return Ok(viewModel);
        }

        /// <summary>
        /// Adds user.
        /// </summary>
        /// <param name = "viewModel" > User view model.</param>
        /// <returns>User identifier.</returns>
        [HttpPost]
        [Authorize(ApplicationPolicies.USER_ADD)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> AddUserAsync([FromBody] CreateUserViewModel viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest();
            }

            var userByEmail = await _userManager.FindByEmailAsync(viewModel.Email);
            if (userByEmail != null)
            {
                return BadRequest("User with specified email already exists.");
            }

            if (!string.IsNullOrEmpty(viewModel.Password))
            {
                foreach (var passwordValidator in _userManager.PasswordValidators)
                {
                    var validationResult = await passwordValidator.ValidateAsync(
                        _userManager, null, viewModel.Password);

                    if (!validationResult.Succeeded)
                    {
                        var errors = validationResult.Errors.Select(x => x.Description).ToList();
                        return BadRequest(string.Join(Environment.NewLine, errors));
                    }
                }
            }

            var user = _mapper.Map<User>(viewModel);
            user.UserName = user.Email;
            user.NormalizedUserName = _userManager.NormalizeName(user.UserName);
            user.NormalizedEmail = _userManager.NormalizeEmail(user.Email);
            user.PhoneNumber = UserPhoneNumber.NormalizePhoneNumber(user.PhoneNumber);
            user.CreateTimeUtc = DateTime.UtcNow;
            if (!string.IsNullOrEmpty(viewModel.Password))
            {
                await _userManager.CreateAsync(user, viewModel.Password);
            }
            else
            {
                await _userManager.CreateAsync(user);
            }

            var userPassword = new UserPassword(user.Id, user.PasswordHash);
            await _userPasswordsRepository.AddAsync(userPassword);

            user = await _userManager.FindByEmailAsync(user.Email);
            if (await _authorizationService.IsAuthorizedAsync(User, ApplicationPolicies.USER_ROLE_UPDATE))
            {
                await _userManager.ChangeRolesAsync(user, viewModel.Roles);
            }

            if (await _authorizationService.IsAuthorizedAsync(User, ApplicationPolicies.USER_CLAIM_UPDATE))
            {
                var claims = _mapper.Map<IEnumerable<Claim>>(viewModel.Claims);
                await _userManager.ChangeClaimsAsync(user, claims);
            }

            await _userStore.UpdateAsync(user, CancellationToken.None);

            return Ok(user.Id);
        }

        /// <summary>
        /// Changes user parameters.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="viewModel">Change user view model.</param>
        /// <returns>Action result.</returns>
        [HttpPut("{userId}")]
        [Authorize(ApplicationPolicies.USER_UPDATE)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> ChangeUserAsync(
            [FromRoute] string userId, [FromBody] ChangeUserViewModel viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest();
            }

            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            _mapper.Map(viewModel, user);
            await _userStore.UpdateAsync(user, CancellationToken.None);

            return NoContent();
        }

        /// <summary>
        /// Deletes user.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>Action result.</returns>
        [HttpDelete("{userId}")]
        [Authorize(ApplicationPolicies.USER_DELETE)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] string userId)
        {
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            await _userManager.DeleteAsync(user);
            await _userStore.UpdateAsync(user, CancellationToken.None);

            return NoContent();
        }

        /// <summary>
        /// Returns user status.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>User status.</returns>
        [HttpGet("{userId}/status")]
        [Authorize(ApplicationPolicies.USER_STATUS_READ)]
        [ProducesResponseType(typeof(UserStatusCode), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserStatusAsync([FromRoute] string userId)
        {
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user.StatusCode);
        }

        /// <summary>
        /// Changes user payment suspension status.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="suspend">Suspend or activate payment service for user.</param>
        /// <returns>Action result.</returns>
        [HttpPost("{userId}/suspend-payment")]
        [Authorize(ApplicationPolicies.USER_STATUS_SUSPEND)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> SuspendPaymentAsync([FromRoute] string userId, [FromBody] bool suspend)
        {
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.StatusCode == UserStatusCode.Blocked)
            {
                return BadRequest("Can not change payment service suspension as long as user is blocked.");
            }

            if (user.StatusCode == UserStatusCode.Active && suspend)
            {
                user.StatusCode = UserStatusCode.PaymentServiceSuspended;
                await _userStore.UpdateAsync(user, CancellationToken.None);
            }
            else if (user.StatusCode == UserStatusCode.PaymentServiceSuspended && !suspend)
            {
                user.StatusCode = UserStatusCode.Active;
                await _userStore.UpdateAsync(user, CancellationToken.None);
            }

            return NoContent();
        }

        /// <summary>
        /// Changes user block status.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="block">Block or unblock user.</param>
        /// <returns>Action result.</returns>
        [HttpPost("{userId}/block")]
        [Authorize(ApplicationPolicies.USER_STATUS_BLOCK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> BlockAsync([FromRoute] string userId, [FromBody] bool block)
        {
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (block)
            {
                user.StatusCode = UserStatusCode.Blocked;
                await _userStore.UpdateAsync(user, CancellationToken.None);
            }
            else if (user.StatusCode == UserStatusCode.Blocked && !block)
            {
                user.StatusCode = UserStatusCode.PaymentServiceSuspended;
                await _userStore.UpdateAsync(user, CancellationToken.None);
            }

            return NoContent();
        }

        /// <summary>
        /// Unlocks user login possibility.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>Action result.</returns>
        [HttpPost("{userId}/unlock")]
        [Authorize(ApplicationPolicies.USER_UNLOCK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> UnlockAsync([FromRoute] string userId)
        {
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.LockoutEnd = null;
            user.AccessFailedCount = 0;
            await _userStore.UpdateAsync(user, CancellationToken.None);

            return NoContent();
        }

        /// <summary>
        /// Returns roles user is in.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>User roles.</returns>
        [HttpGet("{userId}/roles")]
        [Authorize(ApplicationPolicies.USER_ROLE_READ)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRolesAsync([FromRoute] string userId)
        {
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        /// <summary>
        /// Updates roles user is in.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="roles">Roles.</param>
        /// <returns>Action result.</returns>
        [HttpPut("{userId}/roles")]
        [Authorize(ApplicationPolicies.USER_ROLE_UPDATE)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> PutRolesAsync(
            [FromRoute] string userId, [FromBody] IEnumerable<string> roles)
        {
            _logger.LogInformation($"User {userId} roles changing to {string.Join(", ", roles)} started.");

            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            await _userManager.ChangeRolesAsync(user, roles);
            await _userStore.UpdateAsync(user, CancellationToken.None);

            _logger.LogInformation($"User {user.Id} roles changed.");

            return NoContent();
        }

        /// <summary>
        /// Returns user claims.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>User claims.</returns>
        [HttpGet("{userId}/claims")]
        [Authorize(ApplicationPolicies.USER_CLAIM_READ)]
        [ProducesResponseType(typeof(IEnumerable<UserClaimViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetClaimsAsync([FromRoute] string userId)
        {
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var claims = await _userManager.GetClaimsAsync(user);
            var viewModels = _mapper.Map<IEnumerable<UserClaimViewModel>>(claims);

            return Ok(viewModels);
        }

        /// <summary>
        /// Updates user claims.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="viewModels">Claims view models.</param>
        /// <returns>Action result.</returns>
        [HttpPut("{userId}/claims")]
        [Authorize(ApplicationPolicies.USER_CLAIM_UPDATE)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> PutClaimsAsync(
            [FromRoute] string userId, [FromBody] IEnumerable<UserClaimViewModel> viewModels)
        {
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var claims = _mapper.Map<IEnumerable<Claim>>(viewModels);
            await _userManager.ChangeClaimsAsync(user, claims);
            await _userStore.UpdateAsync(user, CancellationToken.None);

            return NoContent();
        }

        /// <summary>
        /// Returns user email.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>User email.</returns>
        [HttpGet("{userId}/email")]
        [Authorize(ApplicationPolicies.USER_EMAIL_READ)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserEmailAsync([FromRoute] string userId)
        {
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user.Email);
        }

        /// <summary>
        /// Changes user email.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="email">Email.</param>
        /// <returns>Action result.</returns>
        [HttpPost("{userId}/email")]
        [Authorize(ApplicationPolicies.USER_EMAIL_UPDATE)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> ChangeEmailAsync([FromRoute] string userId, [FromBody] string email)
        {
            _logger.LogInformation($"User {userId} email changing to {email} started.");

            if (!new EmailAddressAttribute().IsValid(email))
            {
                return BadRequest("Email is invalid.");
            }

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email is not provided.");
            }

            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var userByEmail = await _userManager.FindByEmailAsync(email);
            if (userByEmail != null)
            {
                return BadRequest("User with specified email already exists.");
            }

            user.Email = email;
            user.NormalizedEmail = _userManager.NormalizeEmail(user.Email);
            user.UserName = email;
            user.NormalizedUserName = _userManager.NormalizeName(user.UserName);
            user.EmailConfirmed = false;

            await _userStore.UpdateAsync(user, CancellationToken.None);

            _logger.LogInformation($"User {userId} email changed.");

            return NoContent();
        }

        /// <summary>
        /// Marks user email as confirmed.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>Action result.</returns>
        [HttpPost("{userId}/verify-email")]
        [Authorize(ApplicationPolicies.USER_EMAIL_VERIFY)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> VerifyEmailAsync([FromRoute] string userId)
        {
            _logger.LogInformation($"User {userId} email verification started.");

            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.EmailConfirmed = true;
            await _userStore.UpdateAsync(user, CancellationToken.None);

            _logger.LogInformation($"User {user.Id} email verified.");

            return NoContent();
        }

        /// <summary>
        /// Returns user phone number.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>User phone number.</returns>
        [HttpGet("{userId}/phone")]
        [Authorize(ApplicationPolicies.USER_PHONE_READ)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserPhoneNumberAsync([FromRoute] string userId)
        {
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user.PhoneNumber);
        }

        /// <summary>
        /// Marks user phone number as confirmed.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>Action result.</returns>
        [HttpPost("{userId}/verify-phone")]
        [Authorize(ApplicationPolicies.USER_PHONE_VERIFY)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> VerifyPhoneAsync([FromRoute] string userId)
        {
            _logger.LogInformation($"User {userId} phone verification started.");

            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.PhoneNumberConfirmed = true;
            await _userStore.UpdateAsync(user, CancellationToken.None);

            _logger.LogInformation($"User {user.Id} phone verified.");

            return NoContent();
        }

        /// <summary>
        /// Changes user password to a new one.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="newPassword">New password.</param>
        /// <returns>Action result.</returns>
        [HttpPost("{userId}/password")]
        [Authorize(ApplicationPolicies.USER_PASSWORD_UPDATE)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> ChangePasswordAsync([FromRoute] string userId, [FromBody] string newPassword)
        {
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var validationErrors = new List<string>();
            foreach (var passwordValidator in _userManager.PasswordValidators)
            {
                var result = await passwordValidator.ValidateAsync(_userManager, user, newPassword);
                if (!result.Succeeded)
                {
                    validationErrors.AddRange(result.Errors.Select(x => x.Description));
                }
            }

            if (validationErrors.Any())
            {
                return BadRequest(string.Join(Environment.NewLine, validationErrors));
            }

            var isNewPassword = await _userPasswordsService.CheckPasswordAsync(user, newPassword);
            if (!isNewPassword)
            {
                return BadRequest("Password already been in use.");
            }

            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, newPassword);

            await _userManager.ResetAccessFailedCountAsync(user);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now);

            await _userStore.UpdateAsync(user, CancellationToken.None);

            var userPassword = new UserPassword(user.Id, user.PasswordHash);
            await _userPasswordsService.SaveNewPasswordAsync(user, userPassword);

            return NoContent();
        }

        /// <summary>
        /// Starts reset user password process.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>Action result.</returns>
        [HttpPost("{userId}/reset-password")]
        [Authorize(ApplicationPolicies.USER_PASSWORD_RESET)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> ChangePasswordAsync([FromRoute] string userId)
        {
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            try
            {
                await _usersService.ResetPasswordAsync(userId);
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest("Unable to reset user password");
            }
        }

        private UserInfoViewModel AdjustMasking(UserInfoViewModel viewModel)
        {
            var principal = _principalService.GetPrincipal();

            var canReadEmail = principal?.HasClaim(CommonClaimTypes.CLAIM, ApplicationClaims.USER_EMAIL_READ);
            if (canReadEmail != true)
            {
                viewModel.Email = _maskingDataService.HideSecret(viewModel.Email);
            }

            var canReadPhone = principal?.HasClaim(CommonClaimTypes.CLAIM, ApplicationClaims.USER_PHONE_READ);
            if (canReadPhone != true)
            {
                viewModel.PhoneNumber = _maskingDataService.HideSecret(viewModel.PhoneNumber);
            }

            return viewModel;
        }
    }
}

