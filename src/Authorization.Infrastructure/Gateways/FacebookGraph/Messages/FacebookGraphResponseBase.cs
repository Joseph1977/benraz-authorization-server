using System.Text.Json.Serialization;

namespace Authorization.Infrastructure.Gateways.FacebookGraph.Messages
{
    /// <summary>
    /// Facebook graph response base.
    /// </summary>
    public abstract class FacebookGraphResponseBase
    {
        /// <summary>
        /// Error.
        /// </summary>
        [JsonPropertyName("error")]
        public object Error { get; set; }
    }
}


