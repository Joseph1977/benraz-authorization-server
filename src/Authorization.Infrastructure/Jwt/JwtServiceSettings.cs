using System;

namespace Authorization.Infrastructure.Jwt
{
    /// <summary>
    /// JWT service settings.
    /// </summary>
    public class JwtServiceSettings
    {
        /// <summary>
        /// Issuer.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Token validity period.
        /// </summary>
        public TimeSpan ValidityPeriod { get; set; }

        /// <summary>
        /// Expired password JWT validity period.
        /// </summary>
        public TimeSpan PasswordExpiredValidityPeriod { get; set; }

        /// <summary>
        /// Expired password JWT audience.
        /// </summary>
        public string PasswordExpiredAudience { get; set; }

        /// <summary>
        /// Set new password JWT validity period.
        /// </summary>
        public TimeSpan SetPasswordValidityPeriod { get; set; }

        /// <summary>
        /// Set new password JWT audience.
        /// </summary>
        public string SetPasswordAudience { get; set; }

        /// <summary>
        /// Private key in PEM format for sign tokens.
        /// </summary>
        public string PrivateKeyPem { get; set; }

        /// <summary>
        /// Public key in PEM format for token signature verification.
        /// </summary>
        public string PublicKeyPem { get; set; }

        /// <summary>
        /// Confirm email JWT validity period.
        /// </summary>
        public TimeSpan ConfirmEmailValidityPeriod { get; set; }       
    }
}


