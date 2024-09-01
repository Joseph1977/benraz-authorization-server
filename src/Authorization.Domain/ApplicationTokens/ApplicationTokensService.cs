using Authorization.Domain.Claims;
using Authorization.Infrastructure.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authorization.Domain.ApplicationTokens
{
    /// <summary>
    /// Application tokens service.
    /// </summary>
    public class ApplicationTokensService : IApplicationTokensService
    {
        private readonly IClaimsRepository _claimsRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtService _jwtService;
        private readonly ILogger<ApplicationTokensService> _logger;

        /// <summary>
        /// Creates service.
        /// </summary>
        /// <param name="claimsRepository">Claims repository.</param>
        /// <param name="roleManager">Role manager.</param>
        /// <param name="jwtService">JWT service.</param>
        public ApplicationTokensService(
            IClaimsRepository claimsRepository,
            RoleManager<IdentityRole> roleManager,
            IJwtService jwtService,
            ILogger<ApplicationTokensService> logger)
        {
            _claimsRepository = claimsRepository;
            _roleManager = roleManager;
            _jwtService = jwtService;
            _logger = logger;
        }

        /// <summary>
        /// Creates application token.
        /// </summary>
        /// <param name="request">Create application token request.</param>
        /// <returns>Application token.</returns>
        public async Task<ApplicationToken> CreateAsync(CreateApplicationToken request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Roles == null)
            {
                throw new ArgumentException("Roles not set.", nameof(request.Roles));
            }

            if (request.Claims == null)
            {
                throw new ArgumentException("Claims not set.", nameof(request.Claims));
            }

            var identityRoles = GetIdentityRoles(request.Roles);

            var roleClaims = await GetRoleClaimsAsync(identityRoles);
            var allClaims = roleClaims.Union(request.Claims).ToList();
            var identityClaims = await GetIdentityClaimsAsync(allClaims);
            var customFields = request.CustomFields ?? new List<ApplicationTokenCustomField>();

            var tokenId = Guid.NewGuid();
            var tokenClaims = GetAllClaims(tokenId, identityRoles, identityClaims, customFields);
            var tokenValue = _jwtService.CreateToken(request.Audience, request.ExpirationTimeUtc, tokenClaims);

            _logger.LogInformation($"Token {tokenId} created for application {request.ApplicationId}.");

            var applicationTokenRoles = identityRoles
                .Select(x => new ApplicationTokenRole { RoleId = x.Id })
                .ToList();

            var applicationTokenClaims = identityClaims
                .Select(x => new ApplicationTokenClaim { ClaimId = x.Id })
                .ToList();

            return new ApplicationToken
            {
                Id = tokenId,
                ApplicationId = request.ApplicationId,
                ExpirationTimeUtc = request.ExpirationTimeUtc,
                Name = request.Name,
                Value = tokenValue,
                Roles = applicationTokenRoles,
                Claims = applicationTokenClaims,
                CustomFields = request.CustomFields
            };
        }

        private ICollection<IdentityRole> GetIdentityRoles(IEnumerable<string> roles)
        {
            var identityRoles = _roleManager.Roles.ToList();
            var desiredIdentityRoles = identityRoles.Where(x => roles.Contains(x.Name)).ToList();

            return desiredIdentityRoles;
        }

        private async Task<ICollection<Claim>> GetRoleClaimsAsync(IEnumerable<IdentityRole> identityRoles)
        {
            var roleClaims = new List<Claim>();
            foreach(var identityRole in identityRoles)
            {
                var claims = await _roleManager.GetClaimsAsync(identityRole);
                roleClaims.AddRange(claims);
            }

            return roleClaims;
        }

        private async Task<ICollection<IdentityClaim>> GetIdentityClaimsAsync(IEnumerable<Claim> claims)
        {
            var identityClaims = await _claimsRepository.GetAllAsync();
            var desiredIdentityClaims = identityClaims
                .Where(x => claims.Any(y => x.Type == y.Type && x.Value == y.Value))
                .ToList();

            return desiredIdentityClaims;
        }

        private ICollection<Claim> GetAllClaims(
            Guid id,
            IEnumerable<IdentityRole> identityRoles,
            IEnumerable<IdentityClaim> identityClaims,
            IEnumerable<ApplicationTokenCustomField> customFields)
        {
            var allClaims = new List<Claim>();
            allClaims.Add(new Claim("jti", id.ToString()));
            allClaims.AddRange(identityRoles.Select(x => new Claim("role", x.Name)));
            allClaims.AddRange(identityClaims.Select(x => new Claim(x.Type, x.Value)));
            allClaims.AddRange(customFields.Select(x => new Claim(x.Key, x.Value)));

            return allClaims;
        }
    }
}


