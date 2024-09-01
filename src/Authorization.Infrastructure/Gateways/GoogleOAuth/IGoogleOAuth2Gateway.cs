using Authorization.Infrastructure.Gateways.GoogleOAuth2.Messages;
using System.Threading.Tasks;

namespace Authorization.Infrastructure.Gateways.GoogleOAuth2
{
    /// <summary>
    /// Google OAuth2 gateway.
    /// </summary>
    public interface IGoogleOAuth2Gateway
    {
        /// <summary>
        /// Creates authorization URL.
        /// </summary>
        /// <param name="request">Authorization request.</param>
        /// <returns>Authorization URL.</returns>
        string CreateAuthorizationUrl(AuthorizeRequest request);

        /// <summary>
        /// Sends token request.
        /// </summary>
        /// <param name="request">Token request.</param>
        /// <returns>Token response.</returns>
        Task<TokenResponse> SendAsync(TokenRequest request);
    }
}


