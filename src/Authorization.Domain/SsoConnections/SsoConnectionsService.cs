using Benraz.Infrastructure.Domain.Authorization;
using System;
using System.Threading.Tasks;

namespace Authorization.Domain.SsoConnections
{
    /// <summary>
    /// SSO connections service.
    /// </summary>
    public class SsoConnectionsService : ISsoConnectionsService
    {
        private readonly ISsoConnectionsRepository _ssoConnectionsRepository;

        public SsoConnectionsService(ISsoConnectionsRepository ssoConnectionsRepository)
        {
            _ssoConnectionsRepository = ssoConnectionsRepository;
        }

        /// <summary>
        /// Returns result SSO connection composed from general SSO connection and application-specific connection.
        /// </summary>
        /// <param name="ssoProviderCode">SSO provider code.</param>
        /// <param name="applicationId">Application identifier.</param>
        /// <returns>SSO connection.</returns>
        public async Task<SsoConnection> GetResultConnectionAsync(
            SsoProviderCode ssoProviderCode, Guid? applicationId = null)
        {
            var baseConnection = await _ssoConnectionsRepository.GetAsync(ssoProviderCode);
            var applicationConnection = await _ssoConnectionsRepository.GetAsync(ssoProviderCode, applicationId);
            var resultConnection = baseConnection != null ?
                baseConnection.OverrideWith(applicationConnection) :
                applicationConnection;

            return resultConnection;
        }
    }
}


