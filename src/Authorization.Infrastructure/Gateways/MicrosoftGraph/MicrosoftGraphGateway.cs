using Authorization.Infrastructure.Gateways.MicrosoftGraph.Messages;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Infrastructure.Gateways.MicrosoftGraph
{
    /// <summary>
    /// Microsoft graph gateway.
    /// </summary>
    public class MicrosoftGraphGateway : IMicrosoftGraphGateway
    {
        private readonly MicrosoftGraphGatewaySettings _settings;

        /// <summary>
        /// Creates gateway.
        /// </summary>
        /// <param name="settings">Settings.</param>
        public MicrosoftGraphGateway(IOptionsSnapshot<MicrosoftGraphGatewaySettings> settings)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Sends profile request.
        /// </summary>
        /// <param name="request">Profile request.</param>
        /// <returns>Profile response.</returns>
        public async Task<GetProfileResponse> SendAsync(GetProfileRequest request)
        {
            using (var httpClient = CreateHttpClientFor(request))
            {
                var requestUri = new Uri(new Uri(_settings.BaseUrl), _settings.ProfileEndpoint);
                var responseMessage = await httpClient.GetAsync(requestUri);
                var responseText = await responseMessage.Content.ReadAsStringAsync();

                var response = JsonSerializer.Deserialize<GetProfileResponse>(responseText);
                return response;
            }
        }

        /// <summary>
        /// Sends member groups request.
        /// </summary>
        /// <param name="request">Member groups request.</param>
        /// <returns>Member groups response.</returns>
        public async Task<GetMemberGroupsResponse> SendAsync(GetMemberGroupsRequest request)
        {
            using (var httpClient = CreateHttpClientFor(request))
            {
                var requestUri = new Uri(new Uri(_settings.BaseUrl), _settings.MemberGroupsEndpoint);
                var requestContent = new StringContent(
                    JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
                var responseMessage = await httpClient.PostAsync(requestUri, requestContent);
                var responseText = await responseMessage.Content.ReadAsStringAsync();

                var response = JsonSerializer.Deserialize<GetMemberGroupsResponse>(responseText);
                return response;
            }
        }

        /// <summary>
        /// Sends member of request.
        /// </summary>
        /// <param name="request">Member of request.</param>
        /// <returns>Member of response.</returns>
        public async Task<GetMemberOfResponse> SendAsync(GetMemberOfRequest request)
        {
            using (var httpClient = CreateHttpClientFor(request))
            {
                var requestUri = new Uri(new Uri(_settings.BaseUrl), _settings.MemberOfEndpoint);
                var responseMessage = await httpClient.GetAsync(requestUri);
                var responseText = await responseMessage.Content.ReadAsStringAsync();

                var response = JsonSerializer.Deserialize<GetMemberOfResponse>(responseText);
                return response;
            }
        }

        /// <summary>
        /// Sends transitive member of request.
        /// </summary>
        /// <param name="request">Transitive member of request.</param>
        /// <returns>Member of response.</returns>
        public async Task<GetMemberOfResponse> SendAsync(GetTransitiveMemberOfRequest request)
        {
            using (var httpClient = CreateHttpClientFor(request))
            {
                var requestUri = new Uri(new Uri(_settings.BaseUrl), _settings.TransitiveMemberOfEndpoint);
                var responseMessage = await httpClient.GetAsync(requestUri);
                var responseText = await responseMessage.Content.ReadAsStringAsync();

                var response = JsonSerializer.Deserialize<GetMemberOfResponse>(responseText);
                return response;
            }
        }

        /// <summary>
        /// Sends groups request.
        /// </summary>
        /// <param name="request">Groups request.</param>
        /// <returns>Groups response.</returns>
        public async Task<GetGroupsResponse> SendAsync(GetGroupsRequest request)
        {
            using (var httpClient = CreateHttpClientFor(request))
            {
                var requestUri = new Uri(new Uri(_settings.BaseUrl), _settings.GroupsEndpoint);
                var responseMessage = await httpClient.GetAsync(requestUri);
                var responseText = await responseMessage.Content.ReadAsStringAsync();

                var response = JsonSerializer.Deserialize<GetGroupsResponse>(responseText);
                return response;
            }
        }

        private HttpClient CreateHttpClientFor(MicrosoftGraphRequestBase request)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", request.AccessToken);

            return httpClient;
        }
    }
}


