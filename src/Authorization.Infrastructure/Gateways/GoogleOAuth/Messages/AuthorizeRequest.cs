namespace Authorization.Infrastructure.Gateways.GoogleOAuth2.Messages
{
    /// <summary>
    /// Authorize request.
    /// </summary>
    public class AuthorizeRequest
    {
        /// <summary>
        /// Client identifier.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Scope.
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// Response type.
        /// </summary>
        public string ResponseType { get; set; }

        /// <summary>
        /// Redirect URI.
        /// </summary>
        public string RedirectUri { get; set; }

        /// <summary>
        /// State to be included in the callback.
        /// </summary>
        public string State { get; set; }
    }
}

