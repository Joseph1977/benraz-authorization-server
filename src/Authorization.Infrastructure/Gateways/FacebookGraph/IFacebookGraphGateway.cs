using Authorization.Infrastructure.Gateways.FacebookGraph.Messages;
using System.Threading.Tasks;

namespace Authorization.Infrastructure.Gateways.FacebookGraph
{
    /// <summary>
    /// Facebook graph gateway.
    /// </summary>
    public interface IFacebookGraphGateway
    {
        /// <summary>
        /// Sends user request.
        /// </summary>
        /// <param name="request">User request.</param>
        /// <returns>User response.</returns>
        Task<GetUserResponse> SendAsync(GetUserRequest request);
    }
}


