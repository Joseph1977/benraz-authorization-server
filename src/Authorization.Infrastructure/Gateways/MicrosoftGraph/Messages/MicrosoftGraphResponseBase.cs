using System.Text.Json.Serialization;

namespace Authorization.Infrastructure.Gateways.MicrosoftGraph.Messages
{
    /// <summary>
    /// Microsoft graph response base.
    /// </summary>
    public abstract class MicrosoftGraphResponseBase
    {
        /// <summary>
        /// Error.
        /// </summary>
        [JsonPropertyName("error")]
        public MicrosoftGraphError Error { get; set; }
    }
}


