namespace Authorization.Domain.Emails.ResetPassword
{
    /// <summary>
    /// Reset password email model.
    /// </summary>
    public class ResetPasswordEmailModel : EmailModel
    {
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