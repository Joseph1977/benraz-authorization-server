using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Authorization.Infrastructure.Gateways.MicrosoftGraph.Messages
{
    /// <summary>
    /// Member groups response.
    /// </summary>
    public class GetMemberGroupsResponse : MicrosoftGraphResponseBase
    {
        /// <summary>
        /// Group identifiers.
        /// </summary>
        [JsonPropertyName("value")]
        public ICollection<Guid> GroupIds { get; set; }
    }
}


