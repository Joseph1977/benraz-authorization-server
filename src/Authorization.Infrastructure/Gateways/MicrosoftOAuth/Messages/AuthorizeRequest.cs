namespace Authorization.Infrastructure.Gateways.MicrosoftOAuth2.Messages
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
        /// State to be included in the callback.
        /// </summary>
        public string State { get; set; }
    }
}


