namespace Authorization.Domain.Emails.MfaCode
{
    /// <summary>
    /// Mfa code email model.
    /// </summary>
    public class MfaCodeEmailModel : EmailModel
    {
        /// <summary>
        /// User full name.
        /// </summary>
        public string UserFullName { get; set; }

        /// <summary>
        /// Verification code.
        /// </summary>
        public string VerificationCode { get; set; }

        /// <summary>
        /// Verification code expired minutes.
        /// </summary>
        public int VerificationCodeCooldown { get; set; }

        /// <summary>
        /// Action title.
        /// </summary>
        public string ActionTitle { get; set; }

        /// <summary>
        /// Action title lower case.
        /// </summary>
        public string ActionTitleLC { get; set; } 
    }
}