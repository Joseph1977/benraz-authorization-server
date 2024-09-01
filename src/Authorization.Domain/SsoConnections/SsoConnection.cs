using Authorization.Domain.SsoProviders;
using Benraz.Infrastructure.Common.EntityBase;
using Benraz.Infrastructure.Domain.Authorization;
using System;

namespace Authorization.Domain.SsoConnections
{
    /// <summary>
    /// SSO connection.
    /// </summary>
    public class SsoConnection : AggregateRootBase<Guid>
    {
        /// <summary>
        /// SSO provider code.
        /// </summary>
        public SsoProviderCode SsoProviderCode { get; set; }

        /// <summary>
        /// Application identifier.
        /// Optional, not present for SSO parameters for general usage.
        /// </summary>
        public Guid? ApplicationId { get; set; }

        /// <summary>
        /// Is the connection enabled.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Custom SSO authorization URL.
        /// </summary>
        public string AuthorizationUrl { get; set; }

        /// <summary>
        /// Custom SSO token URL.
        /// </summary>
        public string TokenUrl { get; set; }

        /// <summary>
        /// Custom SSO client identifier.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Custom SSO client secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Custom SSO scope.
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// SSO provider.
        /// </summary>
        public SsoProvider SsoProvider { get; set; }

        /// <summary>
        /// Combines two SSO connections overriding old connection parameters with new one.
        /// </summary>
        /// <param name="connection">SSO connection with higher priority parameters.</param>
        /// <returns>Composite SSO connection.</returns>
        public SsoConnection OverrideWith(SsoConnection connection)
        {
            if (SsoProviderCode != connection?.SsoProviderCode)
            {
                throw new InvalidOperationException("Different SSO providers.");
            }

            return new SsoConnection
            {
                SsoProviderCode = SsoProviderCode,
                ApplicationId = connection?.ApplicationId ?? ApplicationId,
                IsEnabled = connection?.IsEnabled ?? false && IsEnabled,
                AuthorizationUrl = connection?.AuthorizationUrl ?? AuthorizationUrl,
                TokenUrl = connection?.TokenUrl ?? TokenUrl,
                ClientId = connection?.ClientId ?? ClientId,
                ClientSecret = connection?.ClientSecret ?? ClientSecret,
                Scope = connection?.Scope ?? Scope
            };
        }
    }
}


