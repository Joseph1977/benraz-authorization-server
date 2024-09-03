using System.Text.Json.Serialization;

namespace Authorization.Infrastructure.Gateways.FacebookOAuth2.Messages
{
    /// <summary>
    /// Token response.
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// Token type.
        /// </summary>
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// Scope.
        /// </summary>
        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// Expiration period.
        /// </summary>
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Access token.
        /// </summary>
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Error.
        /// </summary>
        [JsonPropertyName("error")]
        public object Error { get; set; }
    }
}


