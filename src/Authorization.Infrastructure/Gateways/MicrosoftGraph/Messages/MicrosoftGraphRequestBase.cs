using System.Text.Json.Serialization;

namespace Authorization.Infrastructure.Gateways.MicrosoftGraph.Messages
{
    /// <summary>
    /// Microsoft graph request base.
    /// </summary>
    public abstract class MicrosoftGraphRequestBase
    {
        /// <summary>
        /// Access token.
        /// </summary>
        [JsonIgnore]
        public string AccessToken { get; set; }
    }
}


