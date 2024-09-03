using System;

namespace Authorization.Domain
{
    /// <summary>
    /// Authorization service settings.
    /// </summary>
    public class AuthorizationServiceSettings
    {
        /// <summary>
        /// Period user can be authorized without email confirmation.
        /// </summary>
        public TimeSpan AuthorizeUnconfirmedEmailPeriod { get; set; }

        /// <summary>
        /// Mfa code length.
        /// </summary>
        public int MfaCodeLength { get; set; }

        /// <summary>
        /// Access token mfa code length.
        /// </summary>
        public int AccessTokenMfaCodeLength { get; set; }

        /// <summary>
        /// Is mfa enabled.
        /// </summary>
        public bool IsMfaEnabled { get; set; }
    }
}


