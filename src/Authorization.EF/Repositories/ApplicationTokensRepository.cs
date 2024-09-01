using Authorization.Domain.ApplicationTokens;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.EF.Repositories
{
    /// <summary>
    /// Application tokens repository.
    /// </summary>
    public class ApplicationTokensRepository :
        AuthorizationRepositoryBase<Guid, ApplicationToken>, IApplicationTokensRepository
    {
        /// <summary>
        /// Creates repository.
        /// </summary>
        /// <param name="context">Context.</param>
        public ApplicationTokensRepository(AuthorizationDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Returns application tokens of an application.
        /// </summary>
        /// <param name="applicationId">Application identifier.</param>
        /// <returns>Application tokens.</returns>
        public async Task<IEnumerable<ApplicationToken>> GetAsync(Guid applicationId)
        {
            return await GetQuery().Where(x => x.ApplicationId == applicationId).ToListAsync();
        }

        protected override IQueryable<ApplicationToken> GetQuery()
        {
            return base.GetQuery()
                .Include(x => x.Claims).ThenInclude(x => x.Claim)
                .Include(x => x.Roles).ThenInclude(x => x.Role)
                .Include(x => x.CustomFields);
        }
    }
}


