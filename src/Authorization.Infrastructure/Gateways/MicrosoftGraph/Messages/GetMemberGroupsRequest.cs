using System.Text.Json.Serialization;

namespace Authorization.Infrastructure.Gateways.MicrosoftGraph.Messages
{
    /// <summary>
    /// Member groups request.
    /// </summary>
    public class GetMemberGroupsRequest : MicrosoftGraphRequestBase
    {
        /// <summary>
        /// Is security enabled only.
        /// </summary>
        [JsonPropertyName("securityEnabledOnly")]
        public bool SecurityEnabledOnly { get; set; }
    }
}


