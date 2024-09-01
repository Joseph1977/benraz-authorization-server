namespace Authorization.Infrastructure.Gateways.FacebookOAuth2
{
    /// <summary>
    /// Facebook OAuth2 gateway settings.
    /// </summary>
    public class FacebookOAuth2GatewaySettings
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


