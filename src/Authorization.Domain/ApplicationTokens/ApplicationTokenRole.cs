using Microsoft.AspNetCore.Identity;
using System;

namespace Authorization.Domain.ApplicationTokens
{
    /// <summary>
    /// Application token role.
    /// </summary>
    public class ApplicationTokenRole
    {
        /// <summary>
        /// Application token identifier.
        /// </summary>
        public Guid ApplicationTokenId { get; set; }

        /// <summary>
        /// Role identifier.
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// Role.
        /// </summary>
        public IdentityRole Role { get; set; }
    }
}


