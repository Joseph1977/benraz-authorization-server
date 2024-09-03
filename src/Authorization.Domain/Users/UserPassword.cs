using Benraz.Infrastructure.Common.EntityBase;
using System;

namespace Authorization.Domain.Users
{
    /// <summary>
    /// User password.
    /// </summary>
    public class UserPassword : AggregateRootBase<Guid>
    {
        /// <summary>
        /// Password requirements regex.
        /// </summary>
        public const string PASSWORD_REGEX = "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9]).{8,}$";

        /// <summary>
        /// User identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Password hash.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Creates user password.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="passwordHash">Password hash.</param>
        public UserPassword(string userId, string passwordHash)
        {
            UserId = userId;
            PasswordHash = passwordHash;
        }

        /// <summary>
        /// Creates user password.
        /// </summary>
        public UserPassword()
        {
        }
    }
}


