using Authorization.Domain.Applications;
using Microsoft.IdentityModel.Tokens;
using Benraz.Infrastructure.Authorization.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Domain.SsoServices
{
    /// <summary>
    /// Access token authentication service.
    /// </summary>
    public class AccessTokenAuthenticationService : IAccessTokenAuthenticationService
    {
        private readonly ITokenValidationService _tokenValidationService;
        private readonly TokenValidator _tokenValidator;

        /// <summary>
        /// Creates service.
        /// </summary>
        /// <param name="tokenValidationService">Token validation service.</param>
        /// <param name="tokenValidator">Token validator.</param>
        public AccessTokenAuthenticationService(
            ITokenValidationService tokenValidationService,
            TokenValidator tokenValidator)
        {
            _tokenValidationService = tokenValidationService;
            _tokenValidator = tokenValidator;
        }

        /// <summary>
        /// Authenticates using access token and returns authorization result.
        /// </summary>
        /// <param name="application">Application.</param>
        /// <param name="accessToken">Access token.</param>
        /// <returns>Authorization result.</returns>
        public Task<SsoAuthorizationResult> AuthenticateAsync(Application application, string accessToken)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            var validationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.FromMinutes(5),
                IssuerSigningKeyResolver = _tokenValidationService.IssuerSigningKeyResolver,
                ValidateAudience = true,
                ValidAudiences = application.Audience?.Split(','),
                AudienceValidator = AudienceValidator,
                ValidateIssuer = true,
                IssuerValidator = _tokenValidationService.IssuerValidator,
            };

            try
            {
                var principal = _tokenValidator.ValidateToken(
                    accessToken, validationParameters, out var validatedToken);

                var result = SsoAuthorizationResult.Authorized(principal.Claims);
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                var result = SsoAuthorizationResult.Unauthorized(ex.Message);
                return Task.FromResult(result);
            }
        }

        private static bool AudienceValidator(
            IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            var newAudiences = validationParameters.ValidAudiences ?? new List<string>();
            return newAudiences.All(x => audiences.Contains(x));
        }
    }
}


