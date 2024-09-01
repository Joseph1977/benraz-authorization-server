using Benraz.Infrastructure.Common.EntityBase;
using Benraz.Infrastructure.Domain.Authorization;

namespace Authorization.Domain.Users
{
    /// <summary>
    /// User status.
    /// </summary>
    public class UserStatus : IEntity<UserStatusCode>
    {
        /// <summary>
        /// User status identifier.
        /// </summary>
        public UserStatusCode Id { get; set; }

        /// <summary>
        /// User status name.
        /// </summary>
        public string Name { get; set; }
    }
}


