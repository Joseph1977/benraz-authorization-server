using Authorization.Domain.SsoConnections;

namespace Authorization.Domain.SsoServices
{
    /// <summary>
    /// SSO service provider.
    /// </summary>
    public interface ISsoServiceProvider
    {
        /// <summary>
        /// Returns SSO service by SSO provider.
        /// </summary>
        /// <param name="ssoProvider">SSO provider.</param>
        /// <returns>SSO service.</returns>
        ISsoService GetSsoService(SsoConnection ssoConnection);
    }
}


