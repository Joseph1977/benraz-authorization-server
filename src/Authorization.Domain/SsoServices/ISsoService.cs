using Authorization.Domain.SsoConnections;
using System.Threading.Tasks;

namespace Authorization.Domain.SsoServices
{
    /// <summary>
    /// SSO service.
    /// </summary>
    public interface ISsoService
    {
        /// <summary>
        /// Creates authorization URL.
        /// </summary>
        /// <param name="ssoConnection">SSO connection.</param>
        /// <param name="returnUrl">Return URL.</param>
        /// <returns>Authorization URL.</returns>
        string CreateAuthorizationUrl(SsoConnection ssoConnection, string returnUrl);

        /// <summary>
        /// Uses authorization code to authorize and return authorization result.
        /// </summary>
        /// <param name="ssoConnection">SSO connection.</param>
        /// <param name="code">Authorization code.</param>
        /// <returns>Authorization result.</returns>
        Task<SsoAuthorizationResult> AuthorizeAsync(SsoConnection ssoConnection, string code);

        /// <summary>
        /// Authorizes using username and password and returns authorization result.
        /// </summary>
        /// <param name="ssoConnection">SSO connection.</param>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        /// <returns>Authorization result.</returns>
        Task<SsoAuthorizationResult> AuthorizeAsync(SsoConnection ssoConnection, string username, string password);
    }
}


