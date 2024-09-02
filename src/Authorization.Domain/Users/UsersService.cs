using Authorization.Domain.Addresses;
using Authorization.Domain.Emails;
using Authorization.Domain.Emails.Confirmation;
using Authorization.Domain.Emails.MfaCode;
using Authorization.Domain.Emails.ResetPassword;
using Authorization.Domain.SsoServices;
using Authorization.Infrastructure.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Benraz.Infrastructure.Common.AccessControl;
using Benraz.Infrastructure.Domain.Authorization;
using Benraz.Infrastructure.Domain.CodeGenerator;
using Benraz.Infrastructure.Domain.Common;
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
        private readonly IUserMfasRepository _userMfasRepository;
        private readonly IInternalSsoService _internalSsoService;
        private readonly IEmailsService _emailsService;
        private readonly IJwtService _jwtService;
        private readonly ILogger<UsersService> _logger;
        private readonly UserServiceSettings _userServiceSettings;
        private readonly JwtServiceSettings _jwtServiceSettings;
        private readonly AuthorizationServiceSettings _authorizationServiceSettings;

        /// <summary>
        /// Creates service.
        /// </summary>
        /// <param name="usersRepository">Users repository.</param>
        /// <param name="userPasswordsRepository">User passwords repository.</param>
        /// <param name="userMfasRepository">User mfas repository.</param>
        /// <param name="internalSsoService">Internal SSO service.</param>
        /// <param name="emailsService">Emails service.</param>
        /// <param name="jwtService">JWT service.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="jwtServiceSettings">JWT service settings.</param>
        /// <param name="userServiceSettings">User service settings.</param>
        /// <param name="authorizationServiceSettings">Authorization service settings.</param>
        public UsersService(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            IUsersRepository usersRepository,
            IUserPasswordsRepository userPasswordsRepository,
            IUserMfasRepository userMfasRepository,
            IInternalSsoService internalSsoService,
            IEmailsService emailsService,
            IJwtService jwtService,
            ILogger<UsersService> logger,
            IOptions<JwtServiceSettings> jwtServiceSettings,
            IOptions<UserServiceSettings> userServiceSettings,
            IOptions<AuthorizationServiceSettings> authorizationServiceSettings)
        {
            _userManager = userManager;
            _userStore = userStore;
            _usersRepository = usersRepository;
            _userPasswordsRepository = userPasswordsRepository;
            _userMfasRepository = userMfasRepository;
            _internalSsoService = internalSsoService;
            _emailsService = emailsService;
            _jwtService = jwtService;
            _logger = logger;
            _jwtServiceSettings = jwtServiceSettings.Value;
            _userServiceSettings = userServiceSettings.Value;
            _authorizationServiceSettings = authorizationServiceSettings.Value;
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

            await SetDefaultUserRoles(user);

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

            var isVerificationCodeValid = await IsVerificationCodeValidAsync(userId, token, MfaCode.ConfirmEmail);
            if (!isVerificationCodeValid)
            {
                _logger.LogInformation($"Email confirmation for user {user.Email} failed - code not valid");
                throw new InvalidOperationException("Failed to confirm email");
            }

            user.EmailConfirmed = true;
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
                await DoResetPasswordAsync(userId, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while user password reset.");
                throw;
            }
        }

        /// <summary>
        /// Get user by email.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <returns>User.</returns>
        public async Task<User> GetUserByEmailAsync(string email, bool? includeEmployee = false)
        {
            return await _usersRepository.GetByEmail(email, includeEmployee);
        }

        /// <summary>
        /// Is verification code valid.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="verificationCode">Verification code.</param>
        /// <param name="mfaCode">Mfa code.</param>
        /// <returns>Is verification code valid.</returns>
        public async Task<bool> IsVerificationCodeValidAsync(string userId, string verificationCode, MfaCode mfaCode)
        {
            var userMfa = await _userMfasRepository.GetUserMfaByUserIdAndCodeTypeAsync(userId, mfaCode);
            if (userMfa is null || userMfa?.ExpirationDate < DateTime.UtcNow || (userMfa.IsCodeConsumed != null && userMfa.IsCodeConsumed.Value))
            {
                return false;
            }

            return userMfa.Code == verificationCode;
        }

        /// <summary>
        /// Get action URL.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="actionType">Action type.</param>
        /// <returns>Action URL.</returns>
        public async Task<string> GetActionUrlParametersAsync(string userId, EmailActionType actionType)
        {
            if (actionType == EmailActionType.ResetPassword)
            {
                return await DoResetPasswordAsync(userId, false);
            }
            else if (actionType == EmailActionType.ConfirmEmail)
            {
                var user = await _usersRepository.GetByIdAsync(userId);
                return await StartEmailConfirmationAsync(user, false);
            }
            return null;
        }

        /// <summary>
        /// Create mfa.
        /// </summary>
        /// <param name="user">User model.</param>
        /// <param name="viewModel">Create mfa view model.</param>
        /// <returns>Create mfa view result.</returns>
        public async Task<CreateMfaViewResult> CreateMfaAsync(User user, CreateMfaViewModel viewModel)
        {
            if (string.IsNullOrEmpty(user.Id))
            {
                throw new ArgumentNullException(nameof(user.Id));
            }

            try
            {
                if (viewModel.Mode == MfaMode.Phone && !user.PhoneNumberConfirmed)
                {
                    throw new InvalidOperationException("can't send to unverified phone");
                }
                if (viewModel.ActionType == MfaCode.ConfirmEmail && viewModel.Mode != MfaMode.Email)
                {
                    throw new InvalidOperationException("can't verify email by sending to phone");
                }
                return await CreateMfaAndSendCodeAsync(user, viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while create mfa.");
                throw;
            }
        }

        /// <summary>
        /// Get one time token by mfa code.
        /// </summary>
        /// <param name="user">User model.</param>
        /// <param name="viewModel">Get token by mfa code model.</param>
        /// <returns>Get token by mfa code result.</returns>
        public async Task<GetTokenByMfaCodeResult> GetOneTimeTokenByMfaCodeAsync(User user, GetTokenByMfaCodeModel viewModel)
        {
            if (string.IsNullOrEmpty(user.Id))
            {
                throw new ArgumentNullException(nameof(user.Id));
            }

            try
            {
                var isVerificationCodeValid = await IsVerificationCodeValidAsync(user.Id, viewModel.Code, viewModel.ActionType);
                if (!isVerificationCodeValid)
                {
                    throw new InvalidOperationException("The code is not valid.");
                }

                var actionValidityPeriod = viewModel.ActionType == MfaCode.ResetPassword ? _jwtServiceSettings.SetPasswordValidityPeriod : _jwtServiceSettings.ConfirmEmailValidityPeriod;
                var mfaCode = await GetUserVerificationCodeAsync(user.Id, viewModel.ActionType, actionValidityPeriod, false, _authorizationServiceSettings.AccessTokenMfaCodeLength, true);

                var response = new GetTokenByMfaCodeResult();
                response.Code = mfaCode;
                response.AccessToken = GetTokenByActionType(user.Id, viewModel.ActionType, mfaCode);
                response.UserId = user.Id;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while create mfa.");
                throw;
            }
        }

        private async Task<string> DoResetPasswordAsync(string userId, bool sendEmail)
        {
            var user = await GetUserAsync(userId);

            var resetPasswordToken = await GetUserVerificationCodeAsync(userId, MfaCode.ResetPassword, _jwtServiceSettings.SetPasswordValidityPeriod, false, _authorizationServiceSettings.AccessTokenMfaCodeLength);

            var claims = new List<Claim>();
            claims.Add(new Claim(CommonClaimTypes.USER_ID, userId));
            claims.Add(new Claim(CommonClaimTypes.CLAIM, ApplicationClaims.PROFILE_PASSWORD_SET));
            var accessToken = _jwtService.CreateSetPasswordToken(null, claims);

            var actionUrlEmailModel = _internalSsoService.CreateSetPasswordUrl(resetPasswordToken, accessToken);

            var resetPasswordEmailModel = new ResetPasswordEmailParameters
            {
                To = user.Email,
                ResetPasswordLink = actionUrlEmailModel,
                UserFullName = user.FullName
            };

            if (sendEmail)
            {
                await _emailsService.SendResetPasswordEmailAsync(resetPasswordEmailModel);
            }

            return actionUrlEmailModel;
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
                Nickname = model.Nickname,
                ZoneInfo = model.ZoneInfo,
                Profile = model.Profile,
                FullName = string.Join(" ", model.FirstName, model.LastName),
                Address = model.Address ?? new Address(),
                PhoneNumber = UserPhoneNumber.NormalizePhoneNumber(model.PhoneNumber)
            };

            user.Address.CreateTimeUtc = DateTime.UtcNow;
            user.Address.Id = Guid.NewGuid();
            user.NormalizedUserName = _userManager.NormalizeName(user.UserName);
            user.NormalizedEmail = _userManager.NormalizeEmail(user.Email);
            user.StatusCode = UserStatusCode.Active;
            user.CreateTimeUtc = DateTime.UtcNow;

            return user;
        }

        /// <summary>
        /// Update user.
        /// </summary>
        /// <param name="user">User model.</param>
        /// <param name="cancellationToken">Cancellation token type.</param>
        /// <returns>Identity result.</returns>
        public async Task<IdentityResult> UpdateUserAsync(User user, CancellationToken cancellationToken)
        {
            user.UpdateTimeUtc = DateTime.UtcNow;
            return await _userStore.UpdateAsync(user, cancellationToken);
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

        private async Task<string> StartEmailConfirmationAsync(User user, bool sendEmail = true)
        {
            var token = await GetUserVerificationCodeAsync(user.Id, MfaCode.ConfirmEmail, _jwtServiceSettings.ConfirmEmailValidityPeriod, false, _authorizationServiceSettings.AccessTokenMfaCodeLength);
            _logger.LogInformation($"Got user {user.Email} token {token}.");

            var confirmEmailAddressUrl = _internalSsoService.CreateConfirmEmailUrl(user.Id, token);
            var confirmEmailModel = new ConfirmationEmailParameters()
            {
                To = user.Email,
                UserFullName = user.FullName,
                ConfirmEmailLink = confirmEmailAddressUrl
            };

            if (sendEmail)
            {
                await _emailsService.SendConfirmationEmailAsync(confirmEmailModel);
            }

            return confirmEmailAddressUrl;
        }

        private async Task<string> GetUserVerificationCodeAsync(string userId, MfaCode mfaCode, TimeSpan validityPeriod, bool isDigitsOnly, int codeLength, bool forceGenerateCode = false)
        {
            var userMfa = await _userMfasRepository.GetUserMfaByUserIdAndCodeTypeAsync(userId, mfaCode);
            string verificationCode;

            if (userMfa is null)
            {
                verificationCode = GenerteMfaCodeByType(isDigitsOnly, codeLength);
                var newUserMfa = new UserMfa()
                {
                    UserId = userId,
                    Code = verificationCode,
                    Type = mfaCode,
                    ExpirationDate = DateTime.UtcNow.Add(validityPeriod)
                };

                await _userMfasRepository.AddAsync(newUserMfa);
            }
            else
            {
                verificationCode = userMfa.UpdateTimeUtc.Add(validityPeriod) > DateTime.UtcNow && !forceGenerateCode ? userMfa.Code : GenerteMfaCodeByType(isDigitsOnly, codeLength);
                userMfa.Code = verificationCode;
                userMfa.ExpirationDate = DateTime.UtcNow.Add(validityPeriod);
                userMfa.IsCodeConsumed = null;
                userMfa.IsSendSuccess = null;
                userMfa.SendThrough = null;
                userMfa.SendError = null;

                await _userMfasRepository.ChangeAsync(userMfa);
            }

            return verificationCode;
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

        private async Task SetDefaultUserRoles(User user)
        {
            var defaultRoles = _userServiceSettings.DefaultRolesList?.Split(',').ToList();

            if (defaultRoles is not null && defaultRoles.Any())
            {
                await _userManager.AddToRolesAsync(user, defaultRoles);
            }
        }

        private async Task<CreateMfaViewResult> CreateMfaAndSendCodeAsync(User user, CreateMfaViewModel viewModel)
        {
            var verificationCodeCooldownSetting = _userServiceSettings.VerificationCodeCooldown;
            var mfaCodeLength = _authorizationServiceSettings.MfaCodeLength;
            var mfaCode = await GetUserVerificationCodeAsync(user.Id, viewModel.ActionType, verificationCodeCooldownSetting, true, mfaCodeLength, true);
            _logger.LogInformation($"Got user {user.Email} MFA token {mfaCode}.");

            var isSendSuccess = true;
            var errorMessage = string.Empty;
            try
            {
                if (viewModel.Mode == MfaMode.Phone)
                {
                    return null;
                }
                else
                {
                    var mfaCodeEmailParameters = new MfaCodeEmailParameters
                    {
                        To = user.Email,
                        VerificationCode = mfaCode,
                        VerificationCodeCooldown = Convert.ToInt32(verificationCodeCooldownSetting.TotalMinutes),
                        UserFullName = user.FullName,
                        ActionTitle = GetActionTitle(viewModel.ActionType)
                    };

                    await _emailsService.SendMfaCodeEmailAsync(mfaCodeEmailParameters);
                    _logger.LogInformation($"Sending: User email= {user.Email} requested MFA for {viewModel.ActionType}");
                }
            }
            catch (Exception e)
            {
                isSendSuccess = false;
                errorMessage = e.ToString();
            }

            var userMfa = await _userMfasRepository.GetUserMfaByUserIdAndCodeTypeAsync(user.Id, viewModel.ActionType);
            userMfa.IsSendSuccess = isSendSuccess;
            userMfa.SendThrough = viewModel.Mode.ToString();
            userMfa.SendError = errorMessage;

            await _userMfasRepository.ChangeAsync(userMfa);

            var validityPeriod = viewModel.ActionType == MfaCode.ResetPassword ? _jwtServiceSettings.SetPasswordValidityPeriod : _jwtServiceSettings.ConfirmEmailValidityPeriod;

            var response = new CreateMfaViewResult();
            response.ExpirationCountDown = verificationCodeCooldownSetting;
            response.Mode = viewModel.Mode.ToString();
            response.MaskedTarget = GetMaskedTarget(user, viewModel.Mode);
            response.AccessToken = GetTokenForValidateMfaCode(user.Id, validityPeriod);
            response.NumberOfDigits = _authorizationServiceSettings.MfaCodeLength;

            return response;
        }

        private string GenerteMfaCodeByType(bool isDigitsOnly, int codeLength)
        {
            return isDigitsOnly ? GenerteDigitsOnlyCode(codeLength) : CodeGenerator.GenerateCode(codeLength);
        }

        private string GenerteDigitsOnlyCode(int codeLength)
        {
            int max = Convert.ToInt32(Math.Pow(10, codeLength)) - 1;
            int min = Convert.ToInt32(Math.Pow(10, codeLength - 1));
            var random = new Random();
            var randomNumber = random.Next(min, max);
            return randomNumber.ToString();
        }

        private string GetMaskedTarget(User user, MfaMode mode)
        {
            if (mode == MfaMode.Phone)
                return $"{GetTextFromString(user.PhoneNumber, 0, 3)}****{GetTextFromString(user.PhoneNumber, -1, user.PhoneNumber.Length - 3)}";

            string[] emailAddressArr = user.Email.Split('@');
            string[] domain = emailAddressArr[1].Split('.');
            return $"{GetTextFromString(emailAddressArr.FirstOrDefault(), 0, 2)}****@{domain.FirstOrDefault()}";
        }

        private string GetTextFromString(string text, int fromPosition, int lastPostition)
        {
            if (fromPosition == -1) return !String.IsNullOrWhiteSpace(text) && text.Length >= lastPostition ? text.Substring(lastPostition) : text;
            return !String.IsNullOrWhiteSpace(text) && text.Length >= lastPostition ? text.Substring(fromPosition, lastPostition) : text;
        }

        private string GetActionTitle(MfaCode actionType)
        {
            switch (actionType)
            {
                case MfaCode.ResetPassword:
                    return "Reset Password";
                case MfaCode.ConfirmEmail:
                    return "Confirm Email";
                default:
                    return actionType.ToString();
            }
        }

        private string GetTokenByActionType(string userId, MfaCode actionType, string mfaCode)
        {
            switch (actionType)
            {
                case MfaCode.ResetPassword:
                    var claims = new List<Claim>();
                    claims.Add(new Claim(CommonClaimTypes.USER_ID, userId));
                    claims.Add(new Claim(CommonClaimTypes.CLAIM, ApplicationClaims.PROFILE_PASSWORD_SET));
                    return _jwtService.CreateSetPasswordToken(null, claims);
                case MfaCode.ConfirmEmail:
                    return mfaCode;
                default:
                    return "";
            }
        }

        private string GetTokenForValidateMfaCode(string userId, TimeSpan validityPeriod)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(CommonClaimTypes.USER_ID, userId));
            claims.Add(new Claim(CommonClaimTypes.CLAIM, ApplicationClaims.PROFILE_MFA_TOKEN));
            return _jwtService.CreateValidateMfaCodeToken(null, claims, validityPeriod);
        }
    }
}