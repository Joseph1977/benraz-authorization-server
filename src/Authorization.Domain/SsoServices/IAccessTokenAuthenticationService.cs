using Authorization.Domain.Applications;
using System.Threading.Tasks;

namespace Authorization.Domain.SsoServices
{
    public interface IAccessTokenAuthenticationService
    {
        /// <summary>
        /// Authenticates using access token and returns authorization result.
        /// </summary>
        /// <param name="application">Application.</param>
        /// <param name="accessToken">Access token.</param>
        /// <returns>Authorization result.</returns>
        Task<SsoAuthorizationResult> AuthenticateAsync(Application application, string accessToken);
    }
}


