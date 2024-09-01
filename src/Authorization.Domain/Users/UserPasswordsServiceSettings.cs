using System;

namespace Authorization.Domain.Users
{
    /// <summary>
    /// User passwords service settings.
    /// </summary>
    public class UserPasswordsServiceSettings
    {
        /// <summary>
        /// User passwords count to keep.
        /// </summary>
        public int KeepUserPasswordsCount { get; set; }

        /// <summary>
        /// Max access failed count.
        /// </summary>
        public int MaxAccessFailedCount { get; set; }

        /// <summary>
        /// Lockout period.
        /// </summary>
        public TimeSpan LockoutPeriod { get; set; }

        /// <summary>
        /// Password expiration period.
        /// </summary>
        public TimeSpan? PasswordExpirationPeriod { get; set; }
    }
}


