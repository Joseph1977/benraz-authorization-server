using Authorization.Infrastructure.Gateways.MicrosoftGraph.Messages;
using System.Threading.Tasks;

namespace Authorization.Infrastructure.Gateways.MicrosoftGraph
{
    /// <summary>
    /// Microsoft graph gateway.
    /// </summary>
    public interface IMicrosoftGraphGateway
    {
        /// <summary>
        /// Sends profile request.
        /// </summary>
        /// <param name="request">Profile request.</param>
        /// <returns>Profile response.</returns>
        Task<GetProfileResponse> SendAsync(GetProfileRequest request);

        /// <summary>
        /// Sends groups request.
        /// </summary>
        /// <param name="request">Groups request.</param>
        /// <returns>Groups response.</returns>
        Task<GetMemberGroupsResponse> SendAsync(GetMemberGroupsRequest request);

        /// <summary>
        /// Sends member of request.
        /// </summary>
        /// <param name="request">Member of request.</param>
        /// <returns>Member of response.</returns>
        Task<GetMemberOfResponse> SendAsync(GetMemberOfRequest request);

        /// <summary>
        /// Sends transitive member of request.
        /// </summary>
        /// <param name="request">Transitive member of request.</param>
        /// <returns>Member of response.</returns>
        Task<GetMemberOfResponse> SendAsync(GetTransitiveMemberOfRequest request);

        /// <summary>
        /// Sends groups request.
        /// </summary>
        /// <param name="request">Groups request.</param>
        /// <returns>Groups response.</returns>
        Task<GetGroupsResponse> SendAsync(GetGroupsRequest request);
    }
}


