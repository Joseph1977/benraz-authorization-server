using System.Text.Json.Serialization;

namespace Authorization.Infrastructure.Gateways.MicrosoftGraph.Messages
{
    /// <summary>
    /// Microsoft graph error.
    /// </summary>
    public class MicrosoftGraphError
    {
        /// <summary>
        /// Error code.
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// Error message.
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }

        /// <summary>
        /// Inner error.
        /// </summary>
        [JsonPropertyName("innerError")]
        public MicrosoftGraphInnerError InnerError { get; set; }
    }
}


