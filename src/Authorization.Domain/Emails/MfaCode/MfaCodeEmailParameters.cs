using System.ComponentModel.DataAnnotations;

namespace Authorization.Domain.Emails.MfaCode
{
    /// <summary>
    /// Mfa code email parameters.
    /// </summary>
    public class MfaCodeEmailParameters
    {
        /// <summary>
        /// To email address.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// User full name.
        /// </summary>
        public string UserFullName { get; set; }

        /// <summary>
        /// Verification code.
        /// </summary>
        public string VerificationCode { get; set; }

        /// <summary>
        /// Verification code cooldown.
        /// </summary>
        public int VerificationCodeCooldown { get; set; }

        /// <summary>
        /// Action title.
        /// </summary>
        public string ActionTitle { get; set; }
    }
}
