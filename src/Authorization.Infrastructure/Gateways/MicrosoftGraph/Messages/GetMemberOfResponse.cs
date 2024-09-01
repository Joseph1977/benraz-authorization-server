using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Authorization.Infrastructure.Gateways.MicrosoftGraph.Messages
{
    /// <summary>
    /// Member of response.
    /// </summary>
    public class GetMemberOfResponse : MicrosoftGraphResponseBase
    {
        /// <summary>
        /// Groups.
        /// </summary>
        [JsonPropertyName("value")]
        public ICollection<MicrosoftGraphGroup> Groups { get; set; }
    }
}


