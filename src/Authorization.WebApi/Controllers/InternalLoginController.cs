using Authorization.Domain;
using Authorization.Domain.SsoServices;
using Authorization.Domain.Users;
using Authorization.EF.Extensions;
using Authorization.WebApi.Authorization;
using Authorization.WebApi.Filtes;
using Authorization.WebApi.Models.InternalLogin;
using Authorization.WebApi.Models.InternalLogin.Response;
using Authorization.WebApi.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Benraz.Infrastructure.Common.AccessControl;
using Benraz.Infrastructure.Domain.Authorization;
using Benraz.Infrastructure.Domain.Common;
using Benraz.Infrastructure.Web.Filters;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Authorization.WebApi.Controllers
{
    /// <summary>
    /// Internal login controller.
    /// </summary>
    [ApiController]
    [ServiceFilter(typeof(LogUsageFilter))]
    [Route("/v{version:ApiVersion}/internal-login")]
    public class InternalLoginController : ControllerBase
    {
        private const int INTERNAL_LOGIN_DELAY_MS = 1000;

        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUsersService _usersService;
        private readonly IUserPasswordsService _userPasswordsService;
        private readonly Domain.IAuthorizationService _authorizationService;
        private readonly IUsageLogsService _usageLogsService;
        private readonly IMapper _mapper;
        private readonly ILogger<InternalLoginController> _logger;
        private readonly UserActionNotificationsServiceSettings _userActionNotificationsServiceSettings;
        private readonly AuthorizationServiceSettings _authorizationServiceSettings;

        /// <summary>
        /// Creates controller.
        /// </summary>
        /// <param name="userManager">User manager.</param>
        /// <param name="userStore">User store.</param>
        /// <param name="usersService">Users service.</param>
        /// <param name="userPasswordsService">User passwords service.</param>
        /// <param name="authorizationService">Authorization service.</param>
        /// <param name="usageLogsService">Usage logs service.</param>
        /// <param name="mapper">Mapper.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="userActionNotificationsServiceSettings">User action notifications service settings.</param>
        /// <param name="authorizationServiceSettings">Authorization service settings.</param>
        public InternalLoginController(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            IUsersService usersService,
            IUserPasswordsService userPasswordsService,
            Domain.IAuthorizationService authorizationService,
            IUsageLogsService usageLogsService,
            IMapper mapper,
            ILogger<InternalLoginController> logger,
            IOptions<UserActionNotificationsServiceSettings> userActionNotificationsServiceSettings,
            IOptions<AuthorizationServiceSettings> authorizationServiceSettings)
        {
            _userManager = userManager;
            _userStore = userStore;
            _usersService = usersService;
            _userPasswordsService = userPasswordsService;
            _authorizationService = authorizationService;
            _usageLogsService = usageLogsService;
            _mapper = mapper;
            _logger = logger;
            _userActionNotificationsServiceSettings = userActionNotificationsServiceSettings.Value;
            _authorizationServiceSettings = authorizationServiceSettings.Value;
        }

        /// <summary>
        /// Processes internal login.
        /// </summary>
        /// <param name="viewModel">View model.</param>
        /// <returns>Login result.</returns>
        /// <remarks>
        /// Used by an internal Authorization server's page where user is asked for his/her login and password.
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(LoginResultViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest("No auth data received");
            }

            var ssoState = SsoState.FromString(viewModel.State);
            try
            {
                await BruteForceProtectAsync();

                var accessTokenResult = await _authorizationService.AuthorizeAsync(
                    ssoState.ApplicationId, SsoProviderCode.Internal, viewModel.Username, viewModel.Password);

                await _usageLogsService.LogUsageAsync(HttpContext, viewModel.Username, "Token successfully created");

                var callbackUrl = await _authorizationService.CreateSuccessCallbackUrlAsync(
                    ssoState.ApplicationId, accessTokenResult.AccessToken, ssoState.ReturnUrl, true /*viewModel.isMfaVerified*/, SsoProviderCode.Internal);

                if (_userActionNotificationsServiceSettings.IsLoginNotifyEnabled)
                {
                    await _authorizationService.SendUserLoginEmailAsync(viewModel.Username);
                }

                var resultViewModel = new LoginResultViewModel
                {
                    AccessToken = accessTokenResult.AccessToken,
                    CallbackUrl = callbackUrl
                };
                return Ok(resultViewModel);
            }
            catch (AuthorizationFailedException ex)
            {
                _logger.LogError(ex, "Error while authorizing user.");
                await _usageLogsService.LogUsageAsync(HttpContext, viewModel.Username, $"Login failed: {ex.Reason}");

                var resultViewModel = new LoginResultViewModel
                {
                    UserId = ex.UserId,
                    Error = ex.Reason,
                    ErrorReasonCode = ex.ReasonCode,
                    MfaRequired = ex.MfaRequired
                };
                return Ok(resultViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while authorizing user.");
                await _usageLogsService.LogUsageAsync(
                    HttpContext, viewModel.Username, $"Login failed: Error while authorizing user");

                var resultViewModel = new LoginResultViewModel { Error = "Unable to authorize user" };
                return Ok(resultViewModel);
            }
        }

        /// <summary>
        /// Signs user up.
        /// </summary>
        /// <param name="viewModel">Sign up view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost("sign-up")]
        [ProducesResponseType(typeof(SignUpResultViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> SignUpAsync([FromBody] SignUpViewModel viewModel)
        {
            await BruteForceProtectAsync();

            var user = await _userManager.FindByNameAsync(viewModel.Username);
            if (user != null)
            {
                return BadRequest("User already registered, please login");
            }

            try
            {
                var model = _mapper.Map<RegisterUserModel>(viewModel);
                var result = await _usersService.RegisterAsync(model);

                var resultViewModel = _mapper.Map<SignUpResultViewModel>(result);
                return Ok(resultViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while signing up user.");
                return BadRequest("Unable to signup user");
            }
        }

        /// <summary>
        /// Sends confirmation email.
        /// </summary>
        /// <param name="viewModel">Send confirmation email view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost("send-confirmation-email")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> SendConfirmationEmailAsync(
            [FromBody] SendConfirmationEmailViewModel viewModel)
        {
            await BruteForceProtectAsync();

            var user = await _userManager.FindByIdAsync(viewModel.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            try
            {
                await _usersService.SendConfirmationEmailAsync(user.Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending confirmation email.");
                return BadRequest("Unable to send confirmation email");
            }
        }

        /// <summary>
        /// Confirms user email.
        /// </summary>
        /// <param name="viewModel">Confirm email view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost("confirm-email")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> ConfirmEmailAsync([FromBody] ConfirmEmailViewModel viewModel)
        {
            await BruteForceProtectAsync();

            var user = await _userManager.FindByIdAsync(viewModel.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            try
            {
                await _usersService.ConfirmEmailAsync(user.Id, viewModel.Token);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while confirming user email.");
                return BadRequest("Unable to confirm email");
            }
        }

        /// <summary>
        /// Starts restore user password process.
        /// </summary>
        /// <param name="viewModel">Restore password view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost("restore-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> RestorePasswordAsync([FromBody] RestorePasswordViewModel viewModel)
        {
            await BruteForceProtectAsync();

            var user = await _userManager.FindByNameAsync(viewModel.Username);
            if (user == null)
            {
                return NotFound("User not found");
            }

            try
            {
                await _usersService.ResetPasswordAsync(user.Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while restoring user password.");
                return BadRequest("Unable to reset user password");
            }
        }

        /// <summary>
        /// Get action URL.
        /// </summary>
        /// <param name="viewModel">Get action URL view model</param>
        /// <returns>Action result</returns>
        [HttpPost("get-action-url")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> GetActionUrlAsync([FromBody] GetActionUrlViewModel viewModel)
        {
            await BruteForceProtectAsync();

            var user = await _userManager.FindByNameAsync(viewModel.UserName);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await _usersService.GetActionUrlParametersAsync(user.Id, viewModel.ActionType);

            return Ok(result);
        }

        /// <summary>
        /// Changes password to a new one.
        /// </summary>
        /// <param name="viewModel">Change password view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost("change-password")]
        [Authorize(ApplicationPolicies.INTERNAL_LOGIN_CHANGE_PASSWORD)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordViewModel viewModel)
        {
            await BruteForceProtectAsync();

            var userId = viewModel.UserId;

            var contextUserId = GetContextUserValue(CommonClaimTypes.USER_ID);
            if (!string.IsNullOrEmpty(contextUserId) &&
                !userId.Equals(contextUserId, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation(
                    $"Change password for user {userId} failed - id from context and from request do not match");
                return BadRequest("User or password is not valid");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogInformation($"Change password for user {userId} failed - user is not found");
                return NotFound("User or password is not valid");
            }

            var isAuthenticated = await _userManager.CheckPasswordAsync(user, viewModel.OldPassword);
            if (!isAuthenticated)
            {
                await _userManager.AccessFailedAsync(user);
                await _userPasswordsService.ProcessAccessFailedAsync(user);

                _logger.LogInformation($"Change password for user {user.Email} failed - incorrect password");
                return BadRequest($"User or password is not valid");
            }

            var validationErrors = await _userManager.ValidatePasswordAsync(user, viewModel.NewPassword);
            if (validationErrors.Any())
            {
                _logger.LogInformation($"Change password for user {user.Email} failed - new password invalid");
                return BadRequest(string.Join(Environment.NewLine, validationErrors));
            }

            var isNewPassword = await _userPasswordsService.CheckPasswordAsync(user, viewModel.NewPassword);
            if (!isNewPassword)
            {
                _logger.LogInformation($"Change password for user {user.Email} failed - password has been used");
                return BadRequest("Password already been in use");
            }

            await _userManager.ChangePasswordAsync(user, viewModel.OldPassword, viewModel.NewPassword);
            await _userStore.UpdateAsync(user, CancellationToken.None);

            var userPassword = new UserPassword(user.Id, user.PasswordHash);
            await _userPasswordsService.SaveNewPasswordAsync(user, userPassword);

            _logger.LogInformation($"Change password for user {user.Email} succeeded");
            return NoContent();
        }

        /// <summary>
        /// Sets new password for user.
        /// </summary>
        /// <param name="viewModel">Set password view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost("set-password")]
        [Authorize(ApplicationPolicies.INTERNAL_LOGIN_SET_PASSWORD)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> SetPasswordAsync([FromBody] SetPasswordViewModel viewModel)
        {
            await BruteForceProtectAsync();

            var userId = GetContextUserValue(CommonClaimTypes.USER_ID);
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogInformation($"Set password failed because token does not have user identifier");
                return Unauthorized("Token is invalid");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogInformation($"Set password for user {userId} failed - user is not found");
                return NotFound("User not found");
            }

            var validationErrors = await _userManager.ValidatePasswordAsync(user, viewModel.NewPassword);
            if (validationErrors.Any())
            {
                _logger.LogInformation($"Change password for user {user.Email} failed - new password invalid");
                return BadRequest(string.Join(Environment.NewLine, validationErrors));
            }

            var isNewPassword = await _userPasswordsService.CheckPasswordAsync(user, viewModel.NewPassword);
            if (!isNewPassword)
            {
                _logger.LogInformation($"Change password for user {user.Email} failed - password has been used");
                return BadRequest("Password already been in use");
            }

            var decodedToken = HttpUtility.UrlDecode(viewModel.Code);
            _logger.LogInformation($"User {userId} decoded token for set password: {decodedToken}.");

            var isVerificationCodeValid = await _usersService.IsVerificationCodeValidAsync(userId, viewModel.Code, MfaCode.ResetPassword);
            if (!isVerificationCodeValid)
            {
                _logger.LogInformation($"Change password for user {user.Email} failed - invalid code.");
                return BadRequest("Failed to change password");
            }

            var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordToken, viewModel.NewPassword);
            if (!result.Succeeded)
            {
                var errorsString = string.Join(", ", result.Errors.Select(x => x.Code));
                _logger.LogInformation($"Change password for user {user.Email} failed - {errorsString}");
                return BadRequest("Failed to change password");
            }

            user.EmailConfirmed = true;

            await _userStore.UpdateAsync(user, CancellationToken.None);

            var userPassword = new UserPassword(user.Id, user.PasswordHash);
            await _userPasswordsService.SaveNewPasswordAsync(user, userPassword);

            _logger.LogInformation($"Set password for user {user.Email} succeeded");
            return NoContent();
        }

        /// <summary>
        /// Create mfa and send to the user.
        /// </summary>
        /// <param name="viewModel">Create mfa view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost("mfa")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> CreateMfaAsync([FromBody] CreateMfaViewModel viewModel)
        {
            await BruteForceProtectAsync();

            var user = await _userManager.FindByNameAsync(viewModel.UserEmail);
            if (user == null)
            {
                return NotFound("User not found");
            }

            try
            {
                var result = await _usersService.CreateMfaAsync(user, viewModel);
                if (result == null)
                    return BadRequest("Not implemented");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while restoring user password.");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get one time token by mfa code.
        /// </summary>
        /// <param name="viewModel">Create mfa view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost("mfa/token")]
        [Authorize(ApplicationPolicies.INTERNAL_LOGIN_MFA_TOKEN)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> GetOneTimeTokenByMfaCodeAsync([FromBody] GetTokenByMfaCodeModel viewModel)
        {
            await BruteForceProtectAsync();

            var user = await _userManager.FindByNameAsync(viewModel.UserEmail);
            if (user == null)
            {
                return NotFound("User not found");
            }

            try
            {
                var result = await _usersService.GetOneTimeTokenByMfaCodeAsync(user, viewModel);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while restoring user password.");
                return BadRequest(ex.Message);
            }
        }

        private Task BruteForceProtectAsync()
        {
            return Task.Delay(INTERNAL_LOGIN_DELAY_MS);
        }

        private string? GetContextUserValue(string claimType)
        {
            return HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == claimType)?.Value;
        }
    }
}
