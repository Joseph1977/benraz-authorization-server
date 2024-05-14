using Authorization.Domain.Claims;
using System;

namespace Authorization.EF.Repositories
{
    /// <summary>
    /// Claims repository.
    /// </summary>
    public class ClaimsRepository : AuthorizationRepositoryBase<Guid, IdentityClaim>, IClaimsRepository
    {
        /// <summary>
        /// Creates repository.
        /// </summary>
        /// <param name="context">Context.</param>
        public ClaimsRepository(AuthorizationDbContext context)
            : base(context)
        {
        }
    }
}


