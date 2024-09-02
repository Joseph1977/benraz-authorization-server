namespace Authorization.Domain.Emails.ResetPassword
{
    /// <summary>
    /// Reset password email parameters.
    /// </summary>
    public class ResetPasswordEmailParameters
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
        /// Reset password link.
        /// </summary>
        public string ResetPasswordLink { get; set; }
    }
}
