namespace Authorization.Domain.Emails
{
    /// <summary>
    /// Reset password email model.
    /// </summary>
    public class ResetPasswordEmailModel
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


