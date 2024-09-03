using System;

namespace Authorization.Domain.Users
{
    /// <summary>
    /// User service settings.
    /// </summary>
    public class UserServiceSettings
    {
        /// <summary>
        /// Default roles list.
        /// </summary>
        public string DefaultRolesList { get; set; }

        /// <summary>
        /// Verification code cooldown.
        /// </summary>
        public TimeSpan VerificationCodeCooldown { get; set; }
    }
}
