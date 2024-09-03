using Authorization.Domain.SsoConnections;
using Authorization.Infrastructure.Gateways.MicrosoftGraph;
using Authorization.Infrastructure.Gateways.MicrosoftGraph.Messages;
using Authorization.Infrastructure.Gateways.MicrosoftOAuth2;
using Authorization.Infrastructure.Gateways.MicrosoftOAuth2.Messages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authorization.Domain.SsoServices
{
    /// <summary>
    /// Microsoft SSO service.
    /// </summary>
    public class MicrosoftSsoService : SsoServiceBase, ISsoService
    {
        private readonly IMicrosoftGraphGateway _microsoftGraphGateway;
        private readonly ILogger<MicrosoftSsoService> _logger;

        /// <summary>
        /// Creates service.
        /// </summary>
        /// <param name="microsoftGraphGateway">Microsoft graph gateway.</param>
        public MicrosoftSsoService(IMicrosoftGraphGateway microsoftGraphGateway,
            ILogger<MicrosoftSsoService> logger)
        {
            _microsoftGraphGateway = microsoftGraphGateway;
            _logger = logger;
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
                GrantType = "authorization_code",
                Code = code
            };

            var token = await CreateOAuth2Gateway(ssoConnection).SendAsync(request);
            var authorizationResult = await CreateAuthorizationResultAsync(token);

            return authorizationResult;
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
            var request = new TokenRequest
            {
                ClientId = ssoConnection.ClientId,
                ClientSecret = ssoConnection.ClientSecret,
                Scope = ssoConnection.Scope,
                GrantType = "password",
                Username = username,
                Password = password
            };

            var token = await CreateOAuth2Gateway(ssoConnection).SendAsync(request);
            var authorizationResult = await CreateAuthorizationResultAsync(token);

            return authorizationResult;
        }

        private async Task<SsoAuthorizationResult> CreateAuthorizationResultAsync(TokenResponse tokenResponse)
        {
            if (string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                return SsoAuthorizationResult.Unauthorized("Cannot retrieve token");
            }

            var claims = await GetClaimsAsync(tokenResponse.AccessToken);
            return SsoAuthorizationResult.Authorized(claims);
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(string accessToken)
        {
            var claims = new List<Claim>();

            var tokenClaims = ExtractClaims(accessToken);
            claims.AddRange(tokenClaims);

            var memberGroupIds = await GetMemberGroupIdsAsync(accessToken);
            if (memberGroupIds != null)
            {
                foreach (var memberGroupId in memberGroupIds)
                {
                    AddClaim(claims, SsoClaimTypes.GROUPS_CLAIM_TYPE, memberGroupId.ToString());
                }

                var allGroups = await GetGroupsAsync(accessToken);
                if (allGroups != null)
                {
                    var memberGroups = allGroups.Where(x => memberGroupIds.Contains(x.Id));
                    foreach (var memberGroup in memberGroups)
                    {
                        AddClaim(claims, SsoClaimTypes.GROUPS_CLAIM_TYPE, memberGroup.DisplayName);
                    }
                }
            }

            var groups = await GetTransitiveMemberOfGroupsAsync(accessToken);
            if (groups != null)
            {
                foreach (var group in groups)
                {
                    AddClaim(claims, SsoClaimTypes.GROUPS_CLAIM_TYPE, group.Id.ToString());
                    AddClaim(claims, SsoClaimTypes.GROUPS_CLAIM_TYPE, group.DisplayName);
                }
            }

            claims = claims.DistinctBy(x => new { x.Type, x.Value }).ToList();

            return claims;
        }

        private async Task<IEnumerable<Guid>> GetMemberGroupIdsAsync(string accessToken)
        {
            var request = new GetMemberGroupsRequest
            {
                AccessToken = accessToken
            };
            var response = await _microsoftGraphGateway.SendAsync(request);

            return response.GroupIds;
        }

        private async Task<IEnumerable<MicrosoftGraphGroup>> GetGroupsAsync(string accessToken)
        {
            var request = new GetGroupsRequest
            {
                AccessToken = accessToken
            };
            var response = await _microsoftGraphGateway.SendAsync(request);

            return response.Groups;
        }

        private async Task<IEnumerable<MicrosoftGraphGroup>> GetTransitiveMemberOfGroupsAsync(string accessToken)
        {
            var request = new GetTransitiveMemberOfRequest
            {
                AccessToken = accessToken
            };
            var response = await _microsoftGraphGateway.SendAsync(request);

            return response.Groups;
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

            var emailClaim = token.Claims.FirstOrDefault(x => x.Type == "email" || x.Type == "upn");
            if (emailClaim != null)
            {
                AddClaim(claims, ClaimTypes.Email, emailClaim.Value);
            }

            return claims;
        }

        private IMicrosoftOAuth2Gateway CreateOAuth2Gateway(SsoConnection ssoConnection)
        {
            var settings = new MicrosoftOAuth2GatewaySettings
            {
                AuthorizeEndpointUrl = ssoConnection.AuthorizationUrl,
                TokenEndpointUrl = ssoConnection.TokenUrl
            };

            return new MicrosoftOAuth2Gateway(settings);
        }
    }
}


