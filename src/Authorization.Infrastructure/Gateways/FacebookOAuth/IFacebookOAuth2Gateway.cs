using Authorization.Infrastructure.Gateways.FacebookOAuth2.Messages;
using System.Threading.Tasks;

namespace Authorization.Infrastructure.Gateways.FacebookOAuth2
{
    /// <summary>
    /// Facebook OAuth2 gateway.
    /// </summary>
    public interface IFacebookOAuth2Gateway
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


