using Authorization.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Benraz.Infrastructure.Domain.Common;
using System;
using System.Threading.Tasks;

namespace Authorization.EF.Repositories
{
    /// <summary>
    /// User mfas repository.
    /// </summary>
    public class UserMfasRepository : AuthorizationRepositoryBase<Guid, UserMfa>, IUserMfasRepository
    {
        /// <summary>
        /// Creates repository.
        /// </summary>
        /// <param name="context">Context.</param>
        public UserMfasRepository(AuthorizationDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Returns user mfa by user identifier and code type.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="mfaCode">Mfa code.</param>
        /// <returns>User passwords.</returns>
        public async Task<UserMfa> GetUserMfaByUserIdAndCodeTypeAsync(string userId, MfaCode mfaCode) 
        {
            return await GetQuery().FirstOrDefaultAsync(x => x.UserId == userId && x.Type == mfaCode);
        }
    }
}
