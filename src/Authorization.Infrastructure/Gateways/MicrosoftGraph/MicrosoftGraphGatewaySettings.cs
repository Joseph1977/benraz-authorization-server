namespace Authorization.Infrastructure.Gateways.MicrosoftGraph
{
    /// <summary>
    /// Microsoft graph gateway settings.
    /// </summary>
    public class MicrosoftGraphGatewaySettings
    {
        /// <summary>
        /// Base URL.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Profile endpoint relative to base URL.
        /// </summary>
        public string ProfileEndpoint { get; set; }

        /// <summary>
        /// Member groups endpoint relative to base URL.
        /// </summary>
        public string MemberGroupsEndpoint { get; set; }

        /// <summary>
        /// Member of endpoint relative to base URL.
        /// </summary>
        public string MemberOfEndpoint { get; set; }

        /// <summary>
        /// Transitive member of endpoint relative to base URL.
        /// </summary>
        public string TransitiveMemberOfEndpoint { get; set; }

        /// <summary>
        /// Groups endpoint relative to base URL.
        /// </summary>
        public string GroupsEndpoint { get; set; }
    }
}


