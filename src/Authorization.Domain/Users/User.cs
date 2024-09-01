using Microsoft.AspNetCore.Identity;
using Benraz.Infrastructure.Domain.Authorization;
using System;
using System.Collections.Generic;

namespace Authorization.Domain.Users
{
    /// <summary>
    /// User.
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// Full name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Status code.
        /// </summary>
        public UserStatusCode StatusCode { get; set; }

        /// <summary>
        /// Create time in UTC.
        /// </summary>
        public DateTime? CreateTimeUtc { get; set; }

        /// <summary>
        /// User roles.
        /// </summary>
        public ICollection<UserRole> UserRoles { get; set; }
    }
}


