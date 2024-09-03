using Authorization.Domain.SsoConnections;
using Microsoft.EntityFrameworkCore;
using Benraz.Infrastructure.Domain.Authorization;
using System;
using System.Threading.Tasks;

namespace Authorization.EF.Repositories
{
    /// <summary>
    /// SSO connections repository.
    /// </summary>
    public class SsoConnectionsRepository : AuthorizationRepositoryBase<Guid, SsoConnection>, ISsoConnectionsRepository
    {
        /// <summary>
        /// Creates repository.
        /// </summary>
        /// <param name="context">Context.</param>
        public SsoConnectionsRepository(AuthorizationDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Returns SSO connection for provider and application.
        /// </summary>
        /// <param name="ssoProviderCode">SSO provider code.</param>
        /// <param name="applicationId">Application identifier.</param>
        /// <returns>SSO connection.</returns>
        public Task<SsoConnection> GetAsync(SsoProviderCode ssoProviderCode, Guid? applicationId = null)
        {
            return GetQuery()
                .FirstOrDefaultAsync(x => x.SsoProviderCode == ssoProviderCode && x.ApplicationId == applicationId);
        }
    }
}


