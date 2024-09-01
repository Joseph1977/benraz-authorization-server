using Authorization.Domain.SsoConnections;
using Benraz.Infrastructure.Domain.Authorization;
using System;

namespace Authorization.Domain.SsoServices
{
    /// <summary>
    /// SSO service provider.
    /// </summary>
    public class SsoServiceProvider : ISsoServiceProvider
    {
        private readonly InternalSsoService _internalSsoService;
        private readonly MicrosoftSsoService _microsoftSsoService;
        private readonly FacebookSsoService _facebookSsoService;
        private readonly GoogleSsoService _googleSsoService;

        /// <summary>
        /// Creates provider.
        /// </summary>
        /// <param name="internalSsoService">Internal SSO service.</param>
        /// <param name="microsoftSsoService">Microsoft SSO service.</param>
        /// <param name="facebookSsoService">Facebook SSO service.</param>
        /// <param name="googleSsoService">Google SSO service.</param>
        public SsoServiceProvider(
            InternalSsoService internalSsoService,
            MicrosoftSsoService microsoftSsoService,
            FacebookSsoService facebookSsoService,
            GoogleSsoService googleSsoService)
        {
            _internalSsoService = internalSsoService;
            _microsoftSsoService = microsoftSsoService;
            _facebookSsoService = facebookSsoService;
            _googleSsoService = googleSsoService;
        }

        /// <summary>
        /// Returns SSO service by SSO provider.
        /// </summary>
        /// <param name="ssoProvider">SSO provider.</param>
        /// <returns>SSO service.</returns>
        public ISsoService GetSsoService(SsoConnection ssoConnection)
        {
            switch (ssoConnection.SsoProviderCode)
            {
                case SsoProviderCode.Internal:
                    return _internalSsoService;

                case SsoProviderCode.Microsoft:
                    return _microsoftSsoService;

                case SsoProviderCode.Facebook:
                    return _facebookSsoService;

                case SsoProviderCode.Google:
                    return _googleSsoService;
            }

            throw new NotSupportedException("SSO provider is not supported.");
        }
    }
}


