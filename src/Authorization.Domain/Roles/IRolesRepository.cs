using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authorization.Domain.Roles
{
    /// <summary>
    /// Roles repository.
    /// </summary>
    public interface IRolesRepository
    {
        /// <summary>
        /// Returns all roles.
        /// </summary>
        /// <returns>Roles.</returns>
        Task<IEnumerable<IdentityRole>> GetAllAsync();

        /// <summary>
        /// Returns role by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Role.</returns>
        Task<IdentityRole> GetByIdAsync(string id);
    }
}


