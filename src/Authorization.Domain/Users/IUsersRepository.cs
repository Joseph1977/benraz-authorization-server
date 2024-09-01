using Benraz.Infrastructure.Common.Paging;
using System.Threading.Tasks;

namespace Authorization.Domain.Users
{
    /// <summary>
    /// Users repository.
    /// </summary>
    public interface IUsersRepository
    {
        /// <summary>
        /// Returns users page.
        /// </summary>
        /// <param name="query">Query.</param>
        /// <returns>Users page.</returns>
        public Task<Page<User>> GetPageAsync(UsersQuery query);
        
        /// <summary>
        /// Returns user by identifier.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>User.</returns>
        public Task<User> GetByIdAsync(string id);
    }
}


