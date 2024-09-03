using System.Collections.Generic;
using System.Linq;

namespace Authorization.Infrastructure.Gateways.FacebookGraph.Messages
{
    /// <summary>
    /// User request.
    /// </summary>
    public class GetUserRequest : FacebookGraphRequestBase
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Fields to request.
        /// </summary>
        public ICollection<string> Fields { get; set; }

        /// <summary>
        /// Creates request.
        /// </summary>
        public GetUserRequest()
        {
            Fields = new List<string>();
        }

        /// <summary>
        /// Returns user endpoint.
        /// </summary>
        /// <returns>Endpoint.</returns>
        public string GetEndpoint()
        {
            var endpoint = Id;
            
            if (Fields != null && Fields.Any())
            {
                endpoint += $"?fields={string.Join(",", Fields)}";
            }

            return endpoint;
        }
    }
}


