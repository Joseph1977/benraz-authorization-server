namespace Authorization.Domain.SsoServices
{
    /// <summary>
    /// Internal SSO service settings.
    /// </summary>
    public class InternalSsoServiceSettings
    {
        /// <summary>
        /// Authorization endpoint URL.
        /// </summary>
        public string AuthorizeEndpointUrl { get; set; }

        /// <summary>
        /// Confirm email endpoint URL.
        /// </summary>
        public string ConfirmEmailEndpointUrl { get; set; }

        /// <summary>
        /// Set password endpoint URL.
        /// </summary>
        public string SetPasswordEndpointUrl { get; set; }
    }
}


