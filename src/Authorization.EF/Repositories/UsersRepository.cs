using Authorization.Domain;
using Authorization.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Benraz.Infrastructure.Common.AccessControl;
using Benraz.Infrastructure.Common.Paging;
using Benraz.Infrastructure.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Authorization.EF.Repositories
{
    /// <summary>
    /// Users repository.
    /// </summary>
    public class UsersRepository : IUsersRepository
    {
        private readonly IPrincipalService _principalService;
        private readonly AuthorizationDbContext _dbContext;

        /// <summary>
        /// Creates repository.
        /// </summary>
        /// <param name="principalService">Principal service.</param>
        /// <param name="dbContext">Database context.</param>
        public UsersRepository(IPrincipalService principalService, AuthorizationDbContext dbContext)
        {
            _principalService = principalService;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns users page.
        /// </summary>
        /// <param name="query">Query.</param>
        /// <returns>Users page.</returns>
        public async Task<Page<User>> GetPageAsync(UsersQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var dbQuery = GetQuery();

            dbQuery = ApplyFiltering(dbQuery, query);
            dbQuery = ApplySorting(dbQuery, query);

            var page = await dbQuery.ToPageAsync(query.PageNo, query.PageSize);
            return page;
        }

        /// <summary>
        /// Returns user by identifier.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>User.</returns>
        public async Task<User> GetByIdAsync(string id)
        {
            return await GetQuery().FirstOrDefaultAsync(x => x.Id == id);
        }

        private IQueryable<User> ApplyFiltering(IQueryable<User> dbQuery, UsersQuery query)
        {
            var filters = GetFilterings(query);
            if (filters.Any())
            {
                var filter = filters.Aggregate((x, y) => !query.Any ? x.AndAlso(y) : x.OrElse(y));
                dbQuery = dbQuery.Where(filter);
            }

            return dbQuery;
        }

        private IEnumerable<Expression<Func<User, bool>>> GetFilterings(UsersQuery query)
        {
            var predicates = new List<Expression<Func<User, bool>>>();

            if (!string.IsNullOrEmpty(query.Filter))
            {
                var phoneNumberFilter = UserPhoneNumber.NormalizePhoneNumber(query.Filter);

                predicates.Add(x => x.Id.Contains(query.Filter) ||
                    x.FullName.Contains(query.Filter) ||
                    x.Email.Contains(query.Filter) ||
                    (!string.IsNullOrEmpty(phoneNumberFilter) && x.PhoneNumber.Contains(phoneNumberFilter)));
            }

            if (!string.IsNullOrEmpty(query.FullName))
            {
                predicates.Add(x => x.FullName.Contains(query.FullName));
            }

            if (!string.IsNullOrEmpty(query.Email))
            {
                predicates.Add(x => x.Email.Contains(query.Email));
            }

            if (!string.IsNullOrEmpty(query.PhoneNumber))
            {
                var phoneNumber = UserPhoneNumber.NormalizePhoneNumber(query.PhoneNumber);

                predicates.Add(x => !string.IsNullOrEmpty(phoneNumber) && x.PhoneNumber.Contains(phoneNumber));
            }

            if (query.EmailConfirmed.HasValue)
            {
                predicates.Add(x => x.EmailConfirmed == query.EmailConfirmed.Value);
            }

            if (query.PhoneNumberConfirmed.HasValue)
            {
                predicates.Add(x => x.PhoneNumberConfirmed == query.PhoneNumberConfirmed.Value);
            }

            if (query.StatusIds != null && query.StatusIds.Any())
            {
                predicates.Add(x => query.StatusIds.Contains(x.StatusCode));
            }

            return predicates;
        }

        private IQueryable<User> ApplySorting(IQueryable<User> dbQuery, UsersQuery query)
        {
            var sorting = GetSorting(query.SortBy);
            dbQuery = !query.SortDesc ? dbQuery.OrderBy(sorting) : dbQuery.OrderByDescending(sorting);

            return dbQuery;
        }

        private static Expression<Func<User, object>> GetSorting(UsersQueryParameter parameter)
        {
            switch (parameter)
            {
                case UsersQueryParameter.Id:
                    return x => x.Id;
                case UsersQueryParameter.FullName:
                    return x => x.FullName;
                case UsersQueryParameter.Email:
                    return x => x.Email;
                case UsersQueryParameter.PhoneNumber:
                    return x => x.PhoneNumber;
                default:
                    return x => x.FullName;
            }
        }

        private IQueryable<User> GetQuery()
        {
            var query = _dbContext.Users.AsQueryable();

            var principal = _principalService.GetPrincipal();

            var canReadEmployees = principal?.HasClaim(CommonClaimTypes.CLAIM, ApplicationClaims.EMPLOYEE_READ);
            if (canReadEmployees != true)
            {
                var employeeRoles = CustomRoles.GetEmployeeRoles();
                foreach (var employeeRole in employeeRoles)
                {
                    query = query.Where(x => !x.UserRoles.Any(y => y.Role.NormalizedName.Contains(employeeRole)));
                }
            }

            return query;
        }
    }
}


