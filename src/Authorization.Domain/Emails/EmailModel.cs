namespace Authorization.Domain.Emails
{
    /// <summary>
    /// Email model.
    /// </summary>
    public class EmailModel
    {
        /// <summary>
        /// Company name.
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Company logo URL.
        /// </summary>
        public string CompanyLogoUrl { get; set; }

        /// <summary>
        /// Company phone number.
        /// </summary>
        public string CompanyPhone { get; set; }

        /// <summary>
        /// Company email.
        /// </summary>
        public string CompanyEmail { get; set; }
    }
}
