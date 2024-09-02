using System;

namespace Authorization.Domain.Users
{
    /// <summary>
    /// Create mfa view result.
    /// </summary>
    public class CreateMfaViewResult
    {
        /// <summary>
        /// Expiration count down.
        /// </summary>
        public TimeSpan ExpirationCountDown { get; set; }

        /// <summary>
        /// Mode to send the code phone/email.
        /// </summary>
        public string Mode { get; set; }

        /// <summary>
        /// Masked target phone number/email address.
        /// </summary>
        public string MaskedTarget { get; set; }

        /// <summary>
        /// Access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Number of digits for mfa code.
        /// </summary>
        public int NumberOfDigits { get; set; }
    }
}
