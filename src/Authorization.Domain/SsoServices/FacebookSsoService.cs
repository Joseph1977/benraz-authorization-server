using Authorization.Domain.SsoConnections;
using Authorization.Infrastructure.Gateways.FacebookGraph;
using Authorization.Infrastructure.Gateways.FacebookGraph.Messages;
using Authorization.Infrastructure.Gateways.FacebookOAuth2;
using Authorization.Infrastructure.Gateways.FacebookOAuth2.Messages;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authorization.Domain.SsoServices
{
    /// <summary>
    /// Facebook SSO service.
    /// </summary>
    public class FacebookSsoService : SsoServiceBase, ISsoService
    {
        private readonly IFacebookGraphGateway _facebookGraphGateway;
        private readonly FacebookSsoServiceSettings _settings;

        /// <summary>
        /// Creates service.
        /// </summary>
        /// <param name="facebookGraphGateway">Facebook graph gateway.</param>
        /// <param name="settings">Facebook SSO service settings.</param>
        public FacebookSsoService(
            IFacebookGraphGateway facebookGraphGateway,
            IOptions<FacebookSsoServiceSettings> settings)
        {
            _facebookGraphGateway = facebookGraphGateway;
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

            var authorizationResult = await CreateAuthorizationResultAsync(response);
            return authorizationResult;
        }

        /// <summary>
        /// Authorizes using username and password and returns authorization result.
        /// </summary>
        /// <param name="ssoConnection">SSO connection.</param>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        /// <returns>Authorization result.</returns>
        /// <remarks>Not supported by Facebook.</remarks>
        public Task<SsoAuthorizationResult> AuthorizeAsync(
            SsoConnection ssoConnection, string username, string password)
        {
            throw new NotSupportedException();
        }

        private async Task<SsoAuthorizationResult> CreateAuthorizationResultAsync(TokenResponse tokenResponse)
        {
            if (tokenResponse.Error != null || string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                return SsoAuthorizationResult.Unauthorized("Cannot retrieve token");
            }

            var claims = await GetClaimsAsync(tokenResponse.AccessToken);
            return SsoAuthorizationResult.Authorized(claims);
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(string accessToken)
        {
            var claims = new List<Claim>();

            var request = new GetUserRequest
            {
                AccessToken = accessToken,
                Id = "me",
                Fields = new List<string> { "name", "email" }
            };

            var response = await _facebookGraphGateway.SendAsync(request);
            if (response == null || response.Error != null)
            {
                return claims;
            }

            AddClaim(claims, ClaimTypes.Name, response.Name);
            AddClaim(claims, ClaimTypes.Email, response.Email);

            return claims;
        }

        private IFacebookOAuth2Gateway CreateOAuth2Gateway(SsoConnection ssoConnection)
        {
            var settings = new FacebookOAuth2GatewaySettings
            {
                AuthorizeEndpointUrl = ssoConnection.AuthorizationUrl,
                TokenEndpointUrl = ssoConnection.TokenUrl
            };

            return new FacebookOAuth2Gateway(settings);
        }
    }
}


