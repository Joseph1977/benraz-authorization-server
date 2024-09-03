using Benraz.Infrastructure.Common.EntityBase;
using System;

namespace Authorization.Domain.Claims
{
    /// <summary>
    /// Claim.
    /// </summary>
    public class IdentityClaim : AggregateRootBase<Guid>
    {
        /// <summary>
        /// Claim type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        public string Value { get; set; }
    }
}


