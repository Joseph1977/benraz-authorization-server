using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Authorization.Infrastructure.Jwt
{
    /// <summary>
    /// JWT service.
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Creates JWT token.
        /// </summary>
        /// <param name="audience">Audience.</param>
        /// <param name="validityPeriod">Validity period.</param>
        /// <param name="claims">Claims to include into token.</param>
        /// <returns>Token.</returns>
        string CreateToken(string audience, TimeSpan? validityPeriod, IEnumerable<Claim> claims);

        /// <summary>
        /// Creates JWT token.
        /// </summary>
        /// <param name="audience">Audience.</param>
        /// <param name="expires">Expires in UTC.</param>
        /// <param name="claims">Claims to include into token.</param>
        /// <returns>Token.</returns>
        string CreateToken(string audience, DateTime? expires, IEnumerable<Claim> claims);

        /// <summary>
        /// Creates JWT token for change password operation.
        /// </summary>
        /// <param name="audience">Audience.</param>
        /// <param name="claims">Claims to include into token.</param>
        /// <returns>Token.</returns>
        string CreatePasswordExpiredToken(string audience, IEnumerable<Claim> claims);

        /// <summary>
        /// Creates JWT token for set password operation.
        /// </summary>
        /// <param name="audience">Audience.</param>
        /// <param name="claims">Claims to include into token.</param>
        /// <returns>Token.</returns>
        string CreateSetPasswordToken(string audience, IEnumerable<Claim> claims);

        /// <summary>
        /// Returns public keys.
        /// </summary>
        /// <returns>Public keys.</returns>
        string GetPublicKeys();

        /// <summary>
        /// Returns auth parameters.
        /// </summary>
        /// <returns>Auth parameters.</returns>
        AuthParameters GetAuthParameters();

        /// <summary>
        /// Creates JWT token for validate mfa code operation.
        /// </summary>
        /// <param name="audience">Audience.</param>
        /// <param name="claims">Claims to include into token.</param>
        /// <param name="validityPeriod">Validity period.</param>
        /// <returns>Token.</returns>
        string CreateValidateMfaCodeToken(string audience, IEnumerable<Claim> claims, TimeSpan validityPeriod);
    }
}


