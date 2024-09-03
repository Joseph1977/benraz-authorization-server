using Benraz.Infrastructure.Common.Repositories;
using Benraz.Infrastructure.Domain.Authorization;
using System;
using System.Threading.Tasks;

namespace Authorization.Domain.SsoConnections
{
    /// <summary>
    /// SSO connections repository.
    /// </summary>
    public interface ISsoConnectionsRepository : IRepository<Guid, SsoConnection>
    {
        /// <summary>
        /// Returns SSO connection for provider and application.
        /// </summary>
        /// <param name="ssoProviderCode">SSO provider code.</param>
        /// <param name="applicationId">Application identifier.</param>
        /// <returns>SSO connection.</returns>
        Task<SsoConnection> GetAsync(SsoProviderCode ssoProviderCode, Guid? applicationId = null);
    }
}


