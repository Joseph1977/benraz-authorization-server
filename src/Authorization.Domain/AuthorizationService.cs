using Authorization.Domain.Applications;
using Authorization.Domain.Emails;
using Authorization.Domain.SsoConnections;
using Authorization.Domain.SsoServices;
using Authorization.Domain.Users;
using Authorization.Infrastructure.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Benraz.Infrastructure.Common.AccessControl;
using Benraz.Infrastructure.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Authorization.Domain
{
    /// <summary>
    /// Authorization service.
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ISsoServiceProvider _ssoServiceProvider;
        private readonly IAccessTokenAuthenticationService _accessTokenAuthenticationService;
        private readonly IJwtService _jwtService;
        private readonly IUsersService _usersService;
        private readonly IApplicationsRepository _applicationsRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly ISsoConnectionsService _ssoConnectionsService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AuthorizationService> _logger;
        private readonly AuthorizationServiceSettings _settings;
        private readonly IEmailsService _emailsService;
        private readonly AuthorizationServiceSettings _authorizationServiceSettings;

        /// <summary>
        /// Creates service.
        /// </summary>
        /// <param name="ssoServiceProvider">SSO service provider.</param>
        /// <param name="accessTokenAuthenticationService">Access token authentication service.</param>
        /// <param name="jwtService">JWT service.</param>
        /// <param name="applicationsRepository">Applications repository.</param>
        /// <param name="ssoConnectionsService">SSO connections service.</param>
        /// <param name="userManager">User manager.</param>
        /// <param name="roleManager">Role manager.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="emailsService">Emails service.</param>
        /// <param name="authorizationServiceSettings">Authorization service settings.</param>
        public AuthorizationService(
            ISsoServiceProvider ssoServiceProvider,
            IAccessTokenAuthenticationService accessTokenAuthenticationService,
            IJwtService jwtService,
            IUsersService usersService,
            IApplicationsRepository applicationsRepository,
            IUsersRepository usersRepository,
            ISsoConnectionsService ssoConnectionsService,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<AuthorizationService> logger,
            IOptions<AuthorizationServiceSettings> settings,
            IEmailsService emailsService,
            IOptions<AuthorizationServiceSettings> authorizationServiceSettings)
        {
            _ssoServiceProvider = ssoServiceProvider;
            _accessTokenAuthenticationService = accessTokenAuthenticationService;
            _jwtService = jwtService;
            _applicationsRepository = applicationsRepository;
            _usersService = usersService;
            _usersRepository = usersRepository;
            _ssoConnectionsService = ssoConnectionsService;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _settings = settings.Value;
            _emailsService = emailsService;
            _authorizationServiceSettings = authorizationServiceSettings.Value;
        }

        /// <summary>
        /// Creates and returns authorization URL.
        /// </summary>
        /// <param name="applicationId">Application identifier.</param>
        /// <param name="ssoProviderCode">SSO provider code.</param>
        /// <param name="returnUrl">Return URL.</param>
        /// <returns>Authorization URL.</returns>
        public async Task<string> CreateAuthorizationUrlAsync(
            Guid applicationId, SsoProviderCode ssoProviderCode, string returnUrl)
        {
            var application = await GetApplicationAsync(applicationId);
            var ssoConnection = await GetSsoConnectionAsync(application, ssoProviderCode);

            try
            {
                var ssoService = _ssoServiceProvider.GetSsoService(ssoConnection);
                var authorizationUrl = ssoService.CreateAuthorizationUrl(ssoConnection, returnUrl);

                return authorizationUrl;
            }
            catch (Exception ex)
            {
                var message = "Error while creating authorization URL for application {0} and SSO provider {1}.";
                _logger.LogError(ex, message, application.Name, ssoProviderCode);

                throw;
            }
        }

        /// <summary>
        /// Completes authorization code authorization flow and returns access token.
        /// </summary>
        /// <param name="applicationId">Application identifier.</param>
        /// <param name="ssoProviderCode">SSO provider code.</param>
        /// <param name="code">Authorization code.</param>
        /// <returns>Access token result.</returns>
        public async Task<AccessTokenResult> AuthorizeAsync(
            Guid applicationId, SsoProviderCode ssoProviderCode, string code)
        {
            var application = await GetApplicationAsync(applicationId);
            var ssoConnection = await GetSsoConnectionAsync(application, ssoProviderCode);

            try
            {
                var ssoService = _ssoServiceProvider.GetSsoService(ssoConnection);

                _logger.LogInformation($"{nameof(ssoService.AuthorizeAsync)} process started.");
                var ssoAuthorizationResult = await ssoService.AuthorizeAsync(ssoConnection, code);
                _logger.LogInformation($"{nameof(ssoService.AuthorizeAsync)} process finished.");

                _logger.LogInformation($"{nameof(ProcessSsoAuthorizationResultAsync)} started.");
                var accessToken = await ProcessSsoAuthorizationResultAsync(application, ssoAuthorizationResult);
                _logger.LogInformation($"{nameof(ProcessSsoAuthorizationResultAsync)} finished.");

                return accessToken;
            }
            catch (Exception ex)
            {
                var message = "Error while processing authorization code for application {0} and SSO provider {1}.";
                _logger.LogError(ex, message, application.Name, ssoProviderCode);

                throw;
            }
        }

        /// <summary>
        /// Completes resource owner password credentials authorization flow and returns access token.
        /// </summary>
        /// <param name="applicationId">Application identifier.</param>
        /// <param name="ssoProviderCode">SSO provider code.</param>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        /// <returns>Access token result.</returns>
        public async Task<AccessTokenResult> AuthorizeAsync(
            Guid applicationId, SsoProviderCode ssoProviderCode, string username, string password)
        {
            var application = await GetApplicationAsync(applicationId);
            var ssoConnection = await GetSsoConnectionAsync(application, ssoProviderCode);

            try
            {
                var ssoService = _ssoServiceProvider.GetSsoService(ssoConnection);
                var ssoAuthorizationResult = await ssoService.AuthorizeAsync(ssoConnection, username, password);

                var accessToken = await ProcessSsoAuthorizationResultAsync(application, ssoAuthorizationResult);

                return accessToken;
            }
            catch (Exception ex)
            {
                var message = "Error while processing username and password for application {0} and SSO provider {1}.";
                _logger.LogError(ex, message, application.Name, ssoProviderCode);

                throw;
            }
        }

        /// <summary>
        /// Processes authorization flow using an existing access token and returns a new one.
        /// </summary>
        /// <param name="applicationId">Application identifier.</param>
        /// <param name="accessToken">Access token.</param>
        /// <returns>Access token result.</returns>
        public async Task<AccessTokenResult> AuthorizeAsync(Guid applicationId, string accessToken)
        {
            var application = await GetApplicationAsync(applicationId);

            try
            {
                var ssoAuthorizationResult = await _accessTokenAuthenticationService
                    .AuthenticateAsync(application, accessToken);

                var newAccessToken = await ProcessSsoAuthorizationResultAsync(application, ssoAuthorizationResult);

                return newAccessToken;
            }
            catch (Exception ex)
            {
                var message = "Error while processing access token for application {0}.";
                _logger.LogError(ex, message, application.Name);

                throw;
            }
        }

        /// <summary>
        /// Creates and returns success callback URL with access token.
        /// </summary>
        /// <param name="applicationId">Application identifier.</param>
        /// <param name="accessToken">Access token.</param>
        /// <param name="returnUrl">Return URL.</param>
        /// <returns>Callback URL.</returns>
        public async Task<string> CreateSuccessCallbackUrlAsync(
            Guid applicationId, string accessToken, string returnUrl, bool? isMfaVerified = false, SsoProviderCode provider = SsoProviderCode.Internal)
        {
            var application = await GetApplicationAsync(applicationId);
            var callbackUrl = application.GetCallbackUrl();
            var fragmentSign = GetUrlFirstFragmentSign(callbackUrl);

            var fragmentParts = new List<string>();
            if (!application.IsAccessTokenFragmentDisabled)
            {
                fragmentParts.Add($"access_token={accessToken}");
            }
            if (!string.IsNullOrEmpty(returnUrl))
            {
                fragmentParts.Add($"returnUrl={returnUrl}");
            }

            var fragment = string.Join("&", fragmentParts);
            if (!string.IsNullOrEmpty(fragment))
            {
                callbackUrl += $"{fragmentSign}{fragment}";
            }

            if (provider == SsoProviderCode.Internal)
            {
                if (_authorizationServiceSettings.IsMfaEnabled && isMfaVerified != true)
                {
                    AuthorizationFailed("MFA needed for login", "MFA needed");
                }
            }

            return callbackUrl;
        }

        /// <summary>
        /// Creates and returns URL for callback with error.
        /// </summary>
        /// <param name="applicationId">Application identifier.</param>
        /// <param name="error">Error.</param>
        /// <returns>Callback URL.</returns>
        public async Task<string> CreateErrorCallbackUrlAsync(Guid applicationId, string error)
        {
            var application = await GetApplicationAsync(applicationId);
            var callbackUrl = application.GetCallbackUrl();
            var fragmentSign = GetUrlFirstFragmentSign(callbackUrl);

            callbackUrl = $"{callbackUrl}{fragmentSign}error={error}";

            return callbackUrl;
        }

        /// <summary>
        /// Send user login email.
        /// </summary>
        /// <param name="username">User name.</param>
        public async Task SendUserLoginEmailAsync(string username)
        {
            var user = await _usersService.GetUserByEmailAsync(username, true);
            await _emailsService.SendUserLoginEmailAsync(user);
        }

        private async Task<Application> GetApplicationAsync(Guid applicationId)
        {
            var application = await _applicationsRepository.GetByIdAsync(applicationId);
            if (application == null)
            {
                AuthorizationFailed($"Application {applicationId} not found.");
            }

            return application;
        }

        private async Task<SsoConnection> GetSsoConnectionAsync(
            Application application, SsoProviderCode ssoProviderCode)
        {
            var ssoConnection = await _ssoConnectionsService.GetResultConnectionAsync(ssoProviderCode, application.Id);
            if (ssoConnection == null || !ssoConnection.IsEnabled)
            {
                AuthorizationFailed($"Application {application.Name} does not support SSO provider {ssoProviderCode}.", ssoProviderCode: ssoProviderCode);
            }

            return ssoConnection;
        }

        private async Task<AccessTokenResult> ProcessSsoAuthorizationResultAsync(
            Application application, SsoAuthorizationResult ssoAuthorizationResult)
        {
            if (ssoAuthorizationResult == null)
            {
                AuthorizationFailed($"Authorization failed for application {application.Name}: no result retrieved.");
            }

            if (ssoAuthorizationResult.IsLockedOut)
            {
                var error = ssoAuthorizationResult.Error ?? "user is locked out";
                AuthorizationFailed(
                    $"Authorization failed for application {application.Name}: {error}.",
                    "You been locked for too many failed attempt, " +
                    "please try again in an hour from your last attempt, or contact our customer support.",
                    AuthorizationFailedReasonCode.Locked);
            }

            if (!ssoAuthorizationResult.IsAuthorized)
            {
                var error = ssoAuthorizationResult.Error ?? "unknown error";
                if (ssoAuthorizationResult.IsInvalidCredentials)
                {
                    AuthorizationFailed(
                        $"Authorization failed for application {application.Name}: {error}.",
                        "Invalid email or password.",
                        AuthorizationFailedReasonCode.InvalidCredentials);
                }

                AuthorizationFailed($"Authorization failed for application {application.Name}: {error}.");
            }

            var user = await GetUserAsync(ssoAuthorizationResult.Claims);
            if (user == null)
            {
                var email = GetUsernameFromClaims(ssoAuthorizationResult.Claims);
                var userFullName = GetUserFullName(ssoAuthorizationResult.Claims);
                if (!string.IsNullOrEmpty(email))
                {
                    var newRegisterModel = new RegisterUserModel
                    {
                        FirstName = userFullName?.Split(' ')?.FirstOrDefault(),
                        LastName = userFullName?.Split(' ')?.LastOrDefault(),
                        Username = email,
                        SendConfirmationEmail = false
                    };
                    var result = await _usersService.RegisterAsync(newRegisterModel);
                    if (string.IsNullOrEmpty(result?.UserId))
                    {
                        AuthorizationFailed($"Authorization failed for application {application.Name}: user not found.");
                    }
                    user = await _usersRepository.GetByIdAsync(result?.UserId);

                    user.EmailConfirmed = true;
                    await _usersService.UpdateUserAsync(user, CancellationToken.None);
                }
                else
                {
                    AuthorizationFailed($"Authorization failed for application {application.Name}: user not found.");
                }
            }

            if (!user.EmailConfirmed && !IsInAuthorizeUnconfirmedEmailPeriod(user))
            {
                AuthorizationFailed(
                    $"Authorization failed for application {application.Name}: " +
                    $"user email {user.Email} is not verified.",
                    "Email is not verified",
                    AuthorizationFailedReasonCode.EmailUnconfirmed,
                    user.Id);
            }

            if (user.StatusCode == UserStatusCode.Blocked)
            {
                AuthorizationFailed(
                    $"Authorization failed for application {application.Name}: user {user.Email} suspended.",
                    "User suspended",
                    AuthorizationFailedReasonCode.Suspended);
            }

            var ssoProviderRelevantClaims = GetSsoProviderRelevantClaims(ssoAuthorizationResult.Claims);

            if (ssoAuthorizationResult.IsPasswordExpired)
            {
                var passwordExpiredClaims = ssoProviderRelevantClaims.ToList();
                passwordExpiredClaims.Add(new Claim(CommonClaimTypes.USER_ID, user.Id));

                var passwordExpiredToken = _jwtService.CreatePasswordExpiredToken(
                    application.Audience, passwordExpiredClaims);
                _logger.LogInformation($"Token for expired password change created for user {user.Email}.");

                return new AccessTokenResult(user.Id, passwordExpiredToken, true);
            }

            var identityClaims = await GetIdentityClaimsAsync(user);
            var resultClaims = identityClaims.Union(ssoProviderRelevantClaims).ToList();

            var token = _jwtService.CreateToken(
                application.Audience, application.AccessTokenValidityPeriod, resultClaims);
            _logger.LogInformation($"Token created for user {user.Email}.");

            return new AccessTokenResult(user.Id, token);
        }

        private async Task<User> GetUserAsync(IEnumerable<Claim> ssoClaims)
        {
            var userIdClaim = ssoClaims.FirstOrDefault(x => x.Type == CommonClaimTypes.USER_ID);
            if (userIdClaim != null)
            {
                return await _userManager.FindByIdAsync(userIdClaim.Value);
            }

            var userName = GetUsernameFromClaims(ssoClaims);
            if (!string.IsNullOrEmpty(userName))
            {
                return await _userManager.FindByEmailAsync(userName);
            }

            return null;
        }

        private string GetUsernameFromClaims(IEnumerable<Claim> ssoClaims)
        {
            var emailClaim = ssoClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            if (emailClaim != null)
            {
                return emailClaim.Value;
            }

            var uniqueNameClaim = ssoClaims.FirstOrDefault(x => x.Type == "unique_name");
            if (uniqueNameClaim != null)
            {
                return uniqueNameClaim.Value;
            }

            return null;
        }

        private string GetUserFullName(IEnumerable<Claim> ssoClaims)
        {
            var fullName = ssoClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            if (fullName != null)
            {
                return fullName.Value;
            }

            return null;
        }

        private async Task<IEnumerable<Claim>> GetIdentityClaimsAsync(User user)
        {
            var identityClaims = new List<Claim>();

            identityClaims.Add(new Claim(CommonClaimTypes.USER_ID, user.Id));
            identityClaims.Add(new Claim(CustomClaimTypes.STATUS, ((int)user.StatusCode).ToString()));

            var userClaims = await _userManager.GetClaimsAsync(user);
            identityClaims.AddRange(userClaims);

            var userRoleNames = await _userManager.GetRolesAsync(user);
            foreach (var roleName in userRoleNames)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    identityClaims.Add(new Claim(ClaimTypes.Role, role.Name));

                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    identityClaims.AddRange(roleClaims);
                }
            }

            identityClaims = identityClaims.DistinctBy(x => new { x.Type, x.Value }).ToList();

            return identityClaims;
        }

        private IEnumerable<Claim> GetSsoProviderRelevantClaims(IEnumerable<Claim> ssoProviderClaims)
        {
            var ssoProviderRelevantClaims = ssoProviderClaims
                .Where(x =>
                    x.Type == ClaimTypes.Name ||
                    x.Type == SsoClaimTypes.IMAGE_URL_CLAIM_TYPE ||
                    x.Type == CustomClaimTypes.IS_PASSWORD_EXPIRED)
                .ToList();

            return ssoProviderRelevantClaims;
        }

        private string GetUrlFirstFragmentSign(string url)
        {
            return url?.Contains("#") == true ? "&" : "#";
        }

        private bool IsInAuthorizeUnconfirmedEmailPeriod(User user)
        {
            return user.CreateTimeUtc >= DateTime.UtcNow - _settings.AuthorizeUnconfirmedEmailPeriod;
        }

        private void AuthorizationFailed(
            string message,
            string reason = "Unable to authorize user",
            AuthorizationFailedReasonCode reasonCode = AuthorizationFailedReasonCode.Unknown,
            string userId = null,
            LogLevel logLevel = LogLevel.Warning,
            SsoProviderCode ssoProviderCode = SsoProviderCode.Internal)
        {
            _logger.Log(logLevel, message);

            var exception = new AuthorizationFailedException(message, reason);
            exception.ReasonCode = reasonCode;
            exception.UserId = userId;
            exception.MfaRequired = ssoProviderCode == SsoProviderCode.Internal ? _authorizationServiceSettings.IsMfaEnabled : false;

            throw exception;
        }
    }
}


