using Authorization.Domain.SsoConnections;
using Authorization.Infrastructure.Gateways.GoogleOAuth2;
using Authorization.Infrastructure.Gateways.GoogleOAuth2.Messages;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authorization.Domain.SsoServices
{
    /// <summary>
    /// Google SSO service.
    /// </summary>
    public class GoogleSsoService : SsoServiceBase, ISsoService
    {
        private readonly GoogleSsoServiceSettings _settings;

        /// <summary>
        /// Creates service.
        /// </summary>
        /// <param name="settings">Google SSO service settings.</param>
        public GoogleSsoService(IOptions<GoogleSsoServiceSettings> settings)
        {
            _settings = settings.Value;
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

            var request = new AuthorizeRequest
            {
                ClientId = ssoConnection.ClientId,
                ResponseType = "code",
                Scope = ssoConnection.Scope,
                RedirectUri = _settings.RedirectUri,
                State = ssoState.ToString()
            };

            var url = CreateOAuth2Gateway(ssoConnection).CreateAuthorizationUrl(request);

            return url;
        }

        /// <summary>
        /// Uses authorization code to authorize and return authorization result.
        /// </summary>
        /// <param name="ssoConnection">SSO connection.</param>
        /// <param name="code">Authorization code.</param>
        /// <returns>Authorization result.</returns>
        public async Task<SsoAuthorizationResult> AuthorizeAsync(SsoConnection ssoConnection, string code)
        {
            var request = new TokenRequest
            {
                ClientId = ssoConnection.ClientId,
                ClientSecret = ssoConnection.ClientSecret,
                Scope = ssoConnection.Scope,
                RedirectUri = _settings.RedirectUri,
                GrantType = "authorization_code",
                Code = code
            };

            var response = await CreateOAuth2Gateway(ssoConnection).SendAsync(request);

            return CreateAuthorizationResult(response);
        }

        /// <summary>
        /// Authorizes using username and password and returns authorization result.
        /// </summary>
        /// <param name="ssoConnection">SSO connection.</param>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        /// <returns>Authorization result.</returns>
        /// <remarks>Not supported by Goole.</remarks>
        public Task<SsoAuthorizationResult> AuthorizeAsync(
            SsoConnection ssoConnection, string username, string password)
        {
            throw new NotSupportedException();
        }

        private SsoAuthorizationResult CreateAuthorizationResult(TokenResponse tokenResponse)
        {
            if (!string.IsNullOrEmpty(tokenResponse.Error))
            {
                return SsoAuthorizationResult.Unauthorized(tokenResponse.Error);
            }

            var claims = ExtractClaims(tokenResponse.IdToken);
            return SsoAuthorizationResult.Authorized(claims);
        }

        private IEnumerable<Claim> ExtractClaims(string accessToken)
        {
            var claims = new List<Claim>();
            var token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);

            var nameClaim = token.Claims.FirstOrDefault(x => x.Type == "name");
            if (nameClaim != null)
            {
                AddClaim(claims, ClaimTypes.Name, nameClaim.Value);
            }

            var emailClaim = token.Claims.FirstOrDefault(x => x.Type == "email");
            if (emailClaim != null)
            {
                AddClaim(claims, ClaimTypes.Email, emailClaim.Value);
            }

            return claims;
        }

        private IGoogleOAuth2Gateway CreateOAuth2Gateway(SsoConnection ssoConnection)
        {
            var settings = new GoogleOAuth2GatewaySettings
            {
                AuthorizeEndpointUrl = ssoConnection.AuthorizationUrl,
                TokenEndpointUrl = ssoConnection.TokenUrl
            };

            return new GoogleOAuth2Gateway(settings);
        }
    }
}


