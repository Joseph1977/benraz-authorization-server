namespace Authorization.Domain.Emails.Confirmation
{
    /// <summary>
    /// Confirmation email parameters.
    /// </summary>
    public class ConfirmationEmailParameters
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
        /// Confirm email link.
        /// </summary>
        public string ConfirmEmailLink { get; set; }
    }
}
