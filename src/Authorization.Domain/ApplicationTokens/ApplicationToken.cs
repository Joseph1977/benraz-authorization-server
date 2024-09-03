using Benraz.Infrastructure.Common.EntityBase;
using System;
using System.Collections.Generic;

namespace Authorization.Domain.ApplicationTokens
{
    /// <summary>
    /// Application token.
    /// </summary>
    public class ApplicationToken : AggregateRootBase<Guid>
    {
        /// <summary>
        /// Application identifier.
        /// </summary>
        public Guid ApplicationId { get; set; }

        /// <summary>
        /// Token name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Token value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Token expiration time in UTC.
        /// </summary>
        public DateTime? ExpirationTimeUtc { get; set; }

        /// <summary>
        /// Created by.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Roles.
        /// </summary>
        public ICollection<ApplicationTokenRole> Roles { get; set; }

        /// <summary>
        /// Claims.
        /// </summary>
        public ICollection<ApplicationTokenClaim> Claims { get; set; }

        /// <summary>
        /// Custom fields.
        /// </summary>
        public ICollection<ApplicationTokenCustomField> CustomFields { get; set; }
    }
}


