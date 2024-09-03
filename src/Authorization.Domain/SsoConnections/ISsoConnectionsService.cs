using Benraz.Infrastructure.Domain.Authorization;
using System;
using System.Threading.Tasks;

namespace Authorization.Domain.SsoConnections
{
    /// <summary>
    /// SSO connections service.
    /// </summary>
    public interface ISsoConnectionsService
    {
        /// <summary>
        /// Returns result SSO connection composed from general SSO connection and application-specific connection.
        /// </summary>
        /// <param name="ssoProviderCode">SSO provider code.</param>
        /// <param name="applicationId">Application identifier.</param>
        /// <returns>SSO connection.</returns>
        Task<SsoConnection> GetResultConnectionAsync(SsoProviderCode ssoProviderCode, Guid? applicationId = null);
    }
}


