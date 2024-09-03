using Authorization.Infrastructure.Gateways.FacebookGraph.Messages;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Authorization.Infrastructure.Gateways.FacebookGraph
{
    /// <summary>
    /// Facebook graph gateway.
    /// </summary>
    public class FacebookGraphGateway : IFacebookGraphGateway
    {
        private readonly FacebookGraphGatewaySettings _settings;

        /// <summary>
        /// Creates gateway.
        /// </summary>
        /// <param name="settings">Settings.</param>
        public FacebookGraphGateway(IOptionsSnapshot<FacebookGraphGatewaySettings> settings)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Sends user request.
        /// </summary>
        /// <param name="request">User request.</param>
        /// <returns>User response.</returns>
        public async Task<GetUserResponse> SendAsync(GetUserRequest request)
        {
            using (var httpClient = CreateHttpClientFor(request))
            {
                var requestUri = new Uri(new Uri(_settings.BaseUrl), request.GetEndpoint());
                var responseMessage = await httpClient.GetAsync(requestUri);
                var responseText = await responseMessage.Content.ReadAsStringAsync();

                var response = JsonSerializer.Deserialize<GetUserResponse>(responseText);
                return response;
            }
        }

        private HttpClient CreateHttpClientFor(FacebookGraphRequestBase request)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", request.AccessToken);

            return httpClient;
        }
    }
}


