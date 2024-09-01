using System.Text.Json.Serialization;
using System;

namespace Authorization.Infrastructure.Gateways.MicrosoftGraph.Messages
{
    /// <summary>
    /// Microsoft graph inner error.
    /// </summary>
    public class MicrosoftGraphInnerError
    {
        /// <summary>
        /// Request identifier.
        /// </summary>
        [JsonPropertyName("request-id")]
        public string RequestId { get; set; }

        /// <summary>
        /// Date.
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }
    }
}


