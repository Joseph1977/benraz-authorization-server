using System;

namespace Authorization.Infrastructure.Gateways.MicrosoftGraph.Messages
{
    /// <summary>
    /// Microsoft graph group.
    /// </summary>
    public class MicrosoftGraphGroup
    {
        /// <summary>
        /// Group identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Group display name.
        /// </summary>
        public string DisplayName { get; set; }
    }
}


