using Authorization.Domain;
using Authorization.Domain.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Benraz.Infrastructure.Common.AccessControl;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.EF.Repositories
{
    /// <summary>
    /// Roles repository.
    /// </summary>
    public class RolesRepository : IRolesRepository
    {
        private readonly IPrincipalService _principalService;
        private readonly AuthorizationDbContext _dbContext;

        /// <summary>
        /// Creates repository.
        /// </summary>
        /// <param name="principalService">Principal service.</param>
        /// <param name="dbContext">Database context.</param>
        public RolesRepository(IPrincipalService principalService, AuthorizationDbContext dbContext)
        {
            _principalService = principalService;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns all roles.
        /// </summary>
        /// <returns>Roles.</returns>
        public async Task<IEnumerable<IdentityRole>> GetAllAsync()
        {
            return await GetQuery().ToListAsync();
        }

        /// <summary>
        /// Returns role by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Role.</returns>
        public async Task<IdentityRole> GetByIdAsync(string id)
        {
            return await GetQuery().FirstOrDefaultAsync(x => x.Id == id);
        }

        private IQueryable<IdentityRole> GetQuery()
        {
            var query = _dbContext.Roles.AsQueryable();

            var principal = _principalService.GetPrincipal();

            var canReadEmployees = principal?.HasClaim(CommonClaimTypes.CLAIM, ApplicationClaims.EMPLOYEE_READ);
            if (canReadEmployees != true)
            {
                var employeeRoles = CustomRoles.GetEmployeeRoles();
                foreach (var employeeRole in employeeRoles)
                {
                    query = query.Where(x => !x.NormalizedName.Contains(employeeRole));
                }
            }

            return query;
        }
    }
}


