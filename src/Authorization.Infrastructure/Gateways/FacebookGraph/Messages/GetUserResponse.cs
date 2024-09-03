using System.Text.Json.Serialization;

namespace Authorization.Infrastructure.Gateways.FacebookGraph.Messages
{
    /// <summary>
    /// User response.
    /// </summary>
    public class GetUserResponse : FacebookGraphResponseBase
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}


