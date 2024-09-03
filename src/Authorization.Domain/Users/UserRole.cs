using Microsoft.AspNetCore.Identity;

namespace Authorization.Domain.Users
{
    /// <summary>
    /// Identity user role.
    /// </summary>
    public class UserRole : IdentityUserRole<string>
    {
        /// <summary>
        /// Identity role.
        /// </summary>
        public IdentityRole Role { get; set; }
    }
}


