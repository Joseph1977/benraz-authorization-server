namespace Authorization.Infrastructure.Gateways.MicrosoftOAuth2.Messages
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
        /// Authorization code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        public string Password { get; set; }
    }
}


