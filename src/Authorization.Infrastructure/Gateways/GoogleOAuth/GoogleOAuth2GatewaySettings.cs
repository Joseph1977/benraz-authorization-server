namespace Authorization.Infrastructure.Gateways.GoogleOAuth2
{
    /// <summary>
    /// Google OAuth2 gateway settings.
    /// </summary>
    public class GoogleOAuth2GatewaySettings
    {
        /// <summary>
        /// Authorization endpoint URL.
        /// </summary>
        public string AuthorizeEndpointUrl { get; set; }

        /// <summary>
        /// Token endpoint URL.
        /// </summary>
        public string TokenEndpointUrl { get; set; }
    }
}


