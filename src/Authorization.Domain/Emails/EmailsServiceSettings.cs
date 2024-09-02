namespace Authorization.Domain.Emails
{
    /// <summary>
    /// Emails service settings.
    /// </summary>
    public class EmailsServiceSettings
    {
        /// <summary>
        /// From email address.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// From display name.
        /// </summary>
        public string FromDisplayName { get; set; }

        /// <summary>
        /// Company name.
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Company logo URL.
        /// </summary>
        public string CompanyLogoUrl { get; set; }

        /// <summary>
        /// Company email.
        /// </summary>
        public string CompanyEmail { get; set; }

        /// <summary>
        /// Company phone number.
        /// </summary>
        public string CompanyPhone { get; set; }

        /// <summary>
        /// Confirm email address email subject.
        /// </summary>
        public string ConfirmEmailSubject { get; set; }

        /// <summary>
        /// Confirm email address email template identifier.
        /// </summary>
        public string ConfirmEmailTemplateId { get; set; }

        /// <summary>
        /// Reset password email subject.
        /// </summary>
        public string ResetPasswordSubject { get; set; }

        /// <summary>
        /// Reset password email template identifier.
        /// </summary>
        public string ResetPasswordTemplateId { get; set; }

        /// <summary>
        /// Mfa code email subject.
        /// </summary>
        public string MfaCodeSubject { get; set; }
    }
}
