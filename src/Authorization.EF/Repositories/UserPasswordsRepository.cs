using Authorization.Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.EF.Repositories
{
    /// <summary>
    /// User passwords repository.
    /// </summary>
    public class UserPasswordsRepository : AuthorizationRepositoryBase<Guid, UserPassword>, IUserPasswordsRepository
    {
        /// <summary>
        /// Creates repository.
        /// </summary>
        /// <param name="context">Context.</param>
        public UserPasswordsRepository(AuthorizationDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Returns passwords by user identifier.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>User passwords.</returns>
        public async Task<IEnumerable<UserPassword>> GetUserPasswordsByUserIdAsync(string userId)
        {
            return await GetQuery().Where(x => x.UserId == userId).ToListAsync();
        }
    }
}


