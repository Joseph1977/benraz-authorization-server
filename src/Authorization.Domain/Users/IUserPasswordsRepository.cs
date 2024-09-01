using Benraz.Infrastructure.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authorization.Domain.Users
{
    /// <summary>
    /// User passwords repository.
    /// </summary>
    public interface IUserPasswordsRepository : IRepository<Guid, UserPassword>
    {
        /// <summary>
        /// Returns user passwords.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>User passwords.</returns>
        Task<IEnumerable<UserPassword>> GetUserPasswordsByUserIdAsync(string userId);
    }
}


