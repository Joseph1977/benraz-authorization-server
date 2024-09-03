namespace Authorization.Domain.Emails.Confirmation
{
    /// <summary>
    /// Confirmation email model.
    /// </summary>
    public class ConfirmationEmailModel : EmailModel
    {
        /// <summary>
        /// User full name.
        /// </summary>
        public string UserFullName { get; set; }

        /// <summary>
        /// Confirm email link.
        /// </summary>
        public string ConfirmEmailLink { get; set; }
    }
}