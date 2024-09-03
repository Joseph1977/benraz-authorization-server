using Authorization.Domain.Claims;
using System;

namespace Authorization.Domain.ApplicationTokens
{
    /// <summary>
    /// Application token claim.
    /// </summary>
    public class ApplicationTokenClaim
    {
        /// <summary>
        /// Application token identifier.
        /// </summary>
        public Guid ApplicationTokenId { get; set; }

        /// <summary>
        /// Claim identifier.
        /// </summary>
        public Guid ClaimId { get; set; }

        /// <summary>
        /// Claim.
        /// </summary>
        public IdentityClaim Claim { get; set; }
    }
}


