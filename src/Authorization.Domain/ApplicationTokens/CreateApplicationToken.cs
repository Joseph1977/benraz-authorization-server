using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Authorization.Domain.ApplicationTokens
{
    /// <summary>
    /// Create application token request.
    /// </summary>
    public class CreateApplicationToken
    {
        /// <summary>
        /// Application identifier.
        /// </summary>
        public Guid ApplicationId { get; set; }

        /// <summary>
        /// Audience.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Expiration time in UTC.
        /// </summary>
        public DateTime? ExpirationTimeUtc { get; set; }

        /// <summary>
        /// Roles.
        /// </summary>
        public ICollection<string> Roles { get; set; }

        /// <summary>
        /// Claims.
        /// </summary>
        public ICollection<Claim> Claims { get; set; }

        /// <summary>
        /// Custom fields.
        /// </summary>
        public ICollection<ApplicationTokenCustomField> CustomFields { get; set; }
    }
}


