namespace Authorization.Infrastructure.Gateways.GoogleOAuth2.Messages
{
    /// <summary>
    /// Token request.
    /// </summary>
    public class TokenRequest
    {
        /// <summary>
        /// Client identifier.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Client secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Grant type.
        /// </summary>
        public string GrantType { get; set; }

        /// <summary>
        /// Scope.
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// Redirect URI.
        /// </summary>
        public string RedirectUri { get; set; }

        /// <summary>
        /// Authorization code.
        /// </summary>
        public string Code { get; set; }
    }
}


