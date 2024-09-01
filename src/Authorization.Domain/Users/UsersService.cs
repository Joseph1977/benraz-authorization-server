using Authorization.Domain.Emails;
using Authorization.Domain.SsoServices;
using Authorization.Infrastructure.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Benraz.Infrastructure.Common.AccessControl;
using Benraz.Infrastructure.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Authorization.Domain.Users
{
    /// <summary>
    /// Users service.
    /// </summary>
    public class UsersService : IUsersService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUsersRepository _usersRepository;
        private readonly IUserPasswordsRepository _userPasswordsRepository;
        private readonly IInternalSsoService _internalSsoService;
        private readonly IEmailsService _emailsService;
        private readonly IJwtService _jwtService;
        private readonly ILogger<UsersService> _logger;

        /// <summary>
        /// Creates service.
        /// </summary>
        /// <param name="usersRepository">Users repository.</param>
        /// <param name="internalSsoService">Internal SSO service.</param>
        /// <param name="emailsService">Emails service.</param>
        /// <param name="jwtService">JWT service.</param>
        /// <param name="logger">Logger.</param>
        public UsersService(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            IUsersRepository usersRepository,
            IUserPasswordsRepository userPasswordsRepository,
            IInternalSsoService internalSsoService,
            IEmailsService emailsService,
            IJwtService jwtService,
            ILogger<UsersService> logger)
        {
            _userManager = userManager;
            _userStore = userStore;
            _usersRepository = usersRepository;
            _userPasswordsRepository = userPasswordsRepository;
            _internalSsoService = internalSsoService;
            _emailsService = emailsService;
            _jwtService = jwtService;
            _logger = logger;
        }

        /// <summary>
        /// Registers new user.
        /// </summary>
        /// <param name="model">Register user model.</param>
        /// <returns>Result.</returns>
        public async Task<RegisterUserResult> RegisterAsync(RegisterUserModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var dbUser = await _userManager.FindByNameAsync(model.Username);
            if (dbUser != null)
            {
                throw new InvalidOperationException("User already exists.");
            }

            var user = CreateUser(model);
            if (!string.IsNullOrEmpty(model.Password))
            {
                await RegisterInternalLoginUserAsync(user, model.Password, model.SendConfirmationEmail);
            }
            else
            {
                await RegisterSsoLoginUserAsync(user);
            }

            var result = new RegisterUserResult
            {
                UserId = user.Id
            };

            return result;
        }

        /// <summary>
        /// Sends email address confirmation email.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>Task.</returns>
        public async Task SendConfirmationEmailAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var user = await GetUserAsync(userId);
            await StartEmailConfirmationAsync(user);
        }

        /// <summary>
        /// Confirms user email.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="token">Email confirmation token.</param>
        /// <returns>Task.</returns>
        public async Task ConfirmEmailAsync(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var user = await GetUserAsync(userId);

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                var errorsString = string.Join(", ", result.Errors.Select(x => x.Code));
                _logger.LogInformation($"Email confirmation for user {user.Email} failed - {errorsString}");
                throw new InvalidOperationException("Failed to confirm email");
            }

            await _userStore.UpdateAsync(user, CancellationToken.None);

            _logger.LogInformation($"Email confirmation for user {user.Email} succeeded");
        }

        /// <summary>
        /// Resets user password.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>Task.</returns>
        public async Task ResetPasswordAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            try
            {
                await DoResetPasswordAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while user password reset.");
                throw;
            }
        }

        private async Task DoResetPasswordAsync(string userId)
        {
            var user = await GetUserAsync(userId);

            var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userStore.UpdateAsync(user, CancellationToken.None);

            var claims = new List<Claim>();
            claims.Add(new Claim(CommonClaimTypes.USER_ID, userId));
            claims.Add(new Claim(CommonClaimTypes.CLAIM, ApplicationClaims.PROFILE_PASSWORD_SET));
            var accessToken = _jwtService.CreateSetPasswordToken(null, claims);

            var setPasswordUrl = _internalSsoService.CreateSetPasswordUrl(resetPasswordToken, accessToken);

            var resetPasswordEmailModel = new ResetPasswordEmailModel
            {
                To = user.Email,
                UserFullName = user.FullName,
                ResetPasswordLink = setPasswordUrl
            };
            await _emailsService.SendResetPasswordEmailAsync(resetPasswordEmailModel);
        }

        private async Task<User> GetUserAsync(string userId)
        {
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            return user;
        }

        private User CreateUser(RegisterUserModel model)
        {
            var user = new User
            {
                UserName = model.Username,
                Email = model.Username,
                FullName = string.Join(" ", model.FirstName, model.LastName),
                PhoneNumber = UserPhoneNumber.NormalizePhoneNumber(model.PhoneNumber)
            };
            user.NormalizedUserName = _userManager.NormalizeName(user.UserName);
            user.NormalizedEmail = _userManager.NormalizeEmail(user.Email);
            user.StatusCode = UserStatusCode.Active;
            user.CreateTimeUtc = DateTime.UtcNow;

            return user;
        }

        private async Task RegisterInternalLoginUserAsync(
            User user, string password, bool sendConfirmationEmail = true)
        {
            var errors = await ValidatePasswordAsync(password);
            if (!string.IsNullOrEmpty(errors))
            {
                throw new InvalidOperationException(errors);
            }

            await _userManager.CreateAsync(user, password);

            var userPassword = new UserPassword(user.Id, user.PasswordHash);
            await _userPasswordsRepository.AddAsync(userPassword);

            if (sendConfirmationEmail)
            {
                await StartEmailConfirmationAsync(user);
            }
        }

        private async Task RegisterSsoLoginUserAsync(User user)
        {
            user.EmailConfirmed = true;
            await _userManager.CreateAsync(user);
        }

        private async Task StartEmailConfirmationAsync(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmEmailAddressUrl = _internalSsoService.CreateConfirmEmailUrl(user.Id, token);
            var confirmEmailModel = new ConfirmationEmailModel
            {
                To = user.Email,
                UserFullName = user.FullName,
                ConfirmEmailLink = confirmEmailAddressUrl
            };

            await _emailsService.SendConfirmationEmailAsync(confirmEmailModel);
        }

        private async Task<string> ValidatePasswordAsync(string password)
        {
            var errors = new List<string>();
            foreach (var passwordValidator in _userManager.PasswordValidators)
            {
                var validationResult = await passwordValidator.ValidateAsync(_userManager, null, password);

                if (!validationResult.Succeeded)
                {
                    var validatorErrors = validationResult.Errors.Select(x => x.Description).ToList();
                    errors.AddRange(validatorErrors);
                }
            }

            if (!errors.Any())
            {
                return null;
            }

            return string.Join(Environment.NewLine, errors);
        }
    }
}


