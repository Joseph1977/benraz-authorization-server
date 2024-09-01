namespace Authorization.Infrastructure.Gateways.MicrosoftOAuth2
{
    /// <summary>
    /// Microsoft OAuth2 gateway settings.
    /// </summary>
    public class MicrosoftOAuth2GatewaySettings
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


