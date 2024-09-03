using Authorization.Domain.SsoConnections;
using Authorization.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authorization.Domain.SsoServices
{
    /// <summary>
    /// Internal SSO service.
    /// </summary>
    public class InternalSsoService : SsoServiceBase, IInternalSsoService
    {
        private readonly InternalSsoServiceSettings _settings;
        private readonly UserManager<User> _userManager;
        private readonly IUserPasswordsService _userPasswordsService;

        /// <summary>
        /// Creates service.
        /// </summary>
        /// <param name="settings">Internal SSO service settings.</param>
        /// <param name="userManager">User manager.</param>
        /// <param name="userPasswordsService">User passwords service.</param>
        public InternalSsoService(
            IOptions<InternalSsoServiceSettings> settings,
            UserManager<User> userManager,
            IUserPasswordsService userPasswordsService)
        {
            _settings = settings.Value;
            _userManager = userManager;
            _userPasswordsService = userPasswordsService;
        }

        /// <summary>
        /// Creates authorization URL.
        /// </summary>
        /// <param name="ssoConnection">SSO connection.</param>
        /// <param name="returnUrl">Return URL.</param>
        /// <returns>Authorization URL.</returns>
        public string CreateAuthorizationUrl(SsoConnection ssoConnection, string returnUrl)
        {
            var ssoState = new SsoState
            {
                ApplicationId = ssoConnection.ApplicationId.Value,
                ReturnUrl = returnUrl
            };

            var format = "{0}?state={1}";
            var url = string.Format(format, _settings.AuthorizeEndpointUrl, ssoState);

            return url;
        }

        /// <summary>
        /// Uses authorization code to authorize and return authorization result.
        /// </summary>
        /// <param name="ssoConnection">SSO connection.</param>
        /// <param name="code">Authorization code.</param>
        /// <returns>Authorization result.</returns>
        /// <remarks>Not supported.</remarks>
        public Task<SsoAuthorizationResult> AuthorizeAsync(SsoConnection ssoConnection, string code)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Authorizes using username and password and returns authorization result.
        /// </summary>
        /// <param name="ssoConnection">SSO connection.</param>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        /// <returns>Authorization result.</returns>
        public async Task<SsoAuthorizationResult> AuthorizeAsync(
            SsoConnection ssoConnection, string username, string password)
        {
            var user = await _userManager.FindByEmailAsync(username);
            if (user == null)
            {
                return SsoAuthorizationResult.InvalidCredentials($"User {username} not found");
            }

            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                return SsoAuthorizationResult.Unauthorized($"User {username} does not support internal login");
            }

            var isLockedOut = await _userManager.IsLockedOutAsync(user);
            if (isLockedOut)
            {
                return SsoAuthorizationResult.LockedOut(
                    $"User {username} had been temporary locked due to exceedence of maximum allowed attemps");
            }

            var isAuthenticated = await _userManager.CheckPasswordAsync(user, password);
            if (isAuthenticated)
            {
                await _userManager.ResetAccessFailedCountAsync(user);
                var claims = ExtractClaims(user);

                var isPasswordExpired = await _userPasswordsService.IsPasswordExpiredAsync(user);
                return isPasswordExpired ?
                    SsoAuthorizationResult.PasswordExpired(claims) :
                    SsoAuthorizationResult.Authorized(claims);
            }

            await _userManager.AccessFailedAsync(user);
            await _userPasswordsService.ProcessAccessFailedAsync(user);
            return SsoAuthorizationResult.InvalidCredentials($"Password is not valid for user {username}");
        }

        /// <summary>
        /// Creates confirm email URL.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="confirmEmailToken">Confirm email token.</param>
        /// <returns>Confirm email URL.</returns>
        public string CreateConfirmEmailUrl(string userId, string confirmEmailToken)
        {
            var url = $"{_settings.ConfirmEmailEndpointUrl}" +
                $"#code={confirmEmailToken}" +
                $"&userId={userId}";

            return url;
        }

        /// <summary>
        /// Creates set password URL.
        /// </summary>
        /// <param name="resetPasswordToken">Reset password token.</param>
        /// <param name="accessToken">Access token to set password.</param>
        /// <returns>Set password URL.</returns>
        public string CreateSetPasswordUrl(string resetPasswordToken, string accessToken)
        {
            var url = $"{_settings.SetPasswordEndpointUrl}" +
                $"#code={resetPasswordToken}" +
                $"&access_token={accessToken}";

            return url;
        }

        private IEnumerable<Claim> ExtractClaims(User user)
        {
            var claims = new List<Claim>();
            AddClaim(claims, ClaimTypes.Name, user.FullName);
            AddClaim(claims, ClaimTypes.Email, user.Email);

            return claims;
        }
    }
}


