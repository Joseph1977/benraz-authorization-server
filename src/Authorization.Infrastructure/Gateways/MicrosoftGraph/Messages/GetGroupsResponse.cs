using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Authorization.Infrastructure.Gateways.MicrosoftGraph.Messages
{
    /// <summary>
    /// Groups response.
    /// </summary>
    public class GetGroupsResponse : MicrosoftGraphResponseBase
    {
        /// <summary>
        /// Groups.
        /// </summary>
        [JsonPropertyName("value")]
        public ICollection<MicrosoftGraphGroup> Groups { get; set; }
    }
}


