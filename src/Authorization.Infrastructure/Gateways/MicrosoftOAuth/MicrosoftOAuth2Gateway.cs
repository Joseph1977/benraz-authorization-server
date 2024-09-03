using Authorization.Infrastructure.Gateways.MicrosoftOAuth2.Messages;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Authorization.Infrastructure.Gateways.MicrosoftOAuth2
{
    /// <summary>
    /// Microsoft OAuth2 gateway.
    /// </summary>
    public class MicrosoftOAuth2Gateway : IMicrosoftOAuth2Gateway
    {
        private readonly MicrosoftOAuth2GatewaySettings _settings;

        /// <summary>
        /// Creates gateway.
        /// </summary>
        /// <param name="settings">Gateway settings.</param>
        public MicrosoftOAuth2Gateway(MicrosoftOAuth2GatewaySettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Creates authorization URL.
        /// </summary>
        /// <param name="request">Authorization request.</param>
        /// <returns>Authorization URL.</returns>
        public string CreateAuthorizationUrl(AuthorizeRequest request)
        {
            var format = "{0}?response_type={1}&client_id={2}&scope={3}&state={4}";
            var url = string.Format(
                format,
                _settings.AuthorizeEndpointUrl, request.ResponseType, request.ClientId, request.Scope, request.State);

            return url;
        }

        /// <summary>
        /// Sends token request.
        /// </summary>
        /// <param name="request">Token request.</param>
        /// <returns>Token response.</returns>
        public async Task<TokenResponse> SendAsync(TokenRequest request)
        {
            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", request.ClientId),
                new KeyValuePair<string, string>("client_secret", request.ClientSecret),
                new KeyValuePair<string, string>("grant_type", request.GrantType),
                new KeyValuePair<string, string>("scope", request.Scope),
                new KeyValuePair<string, string>("code", request.Code),
                new KeyValuePair<string, string>("username", request.Username),
                new KeyValuePair<string, string>("password", request.Password)
            };
            var actualParameters = parameters.Where(x => !string.IsNullOrEmpty(x.Value)).ToList();

            using (var httpClient = new HttpClient())
            {
                var content = new FormUrlEncodedContent(actualParameters);
                var response = await httpClient.PostAsync(_settings.TokenEndpointUrl, content);
                var responseText = await response.Content.ReadAsStringAsync();

                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseText);
                return tokenResponse;
            }
        }
    }
}


