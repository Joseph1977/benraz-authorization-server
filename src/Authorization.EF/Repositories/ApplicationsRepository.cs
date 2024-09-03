using Authorization.Domain.Applications;
using Microsoft.EntityFrameworkCore;
using Benraz.Infrastructure.Common.Paging;
using Benraz.Infrastructure.EF;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Authorization.EF.Repositories
{
    /// <summary>
    /// Applications repository.
    /// </summary>
    public class ApplicationsRepository : AuthorizationRepositoryBase<Guid, Application>, IApplicationsRepository
    {
        /// <summary>
        /// Creates repository.
        /// </summary>
        /// <param name="context">Context.</param>
        public ApplicationsRepository(AuthorizationDbContext context) 
            : base(context)
        {
        }

        /// <summary>
        /// Returns applications page.
        /// </summary>
        /// <param name="query">Applications query.</param>
        /// <returns>Applications page.</returns>
        public async Task<Page<Application>> GetPageAsync(ApplicationsQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            IQueryable<Application> dbQuery = GetQuery();

            if (!string.IsNullOrEmpty(query.Filter))
            {
                dbQuery = dbQuery.Where(x => 
                    x.Id.ToString().Contains(query.Filter) ||
                    x.Name.Contains(query.Filter) ||
                    x.CreatedBy.Contains(query.Filter) ||
                    x.UpdatedBy.Contains(query.Filter));
            }

            var sorting = GetSorting(query.SortBy);
            dbQuery = !query.SortDesc ? dbQuery.OrderBy(sorting) : dbQuery.OrderByDescending(sorting);

            var page = await dbQuery.ToPageAsync(query.PageNo, query.PageSize);
            return page;
        }

        protected override IQueryable<Application> GetQuery()
        {
            return base.GetQuery()
                .Include(x => x.SsoConnections)
                .Include(x => x.Urls);
        }

        private Expression<Func<Application, object>> GetSorting(ApplicationsQueryParameter parameter)
        {
            switch (parameter)
            {
                case ApplicationsQueryParameter.Id:
                    return x => x.Id.ToString();
                case ApplicationsQueryParameter.Name:
                    return x => x.Name;
                case ApplicationsQueryParameter.Audience:
                    return x => x.Audience;
                case ApplicationsQueryParameter.CreateTimeUtc:
                    return x => x.CreateTimeUtc;
                case ApplicationsQueryParameter.UpdateTimeUtc:
                    return x => x.UpdateTimeUtc;
                case ApplicationsQueryParameter.CreatedBy:
                    return x => x.CreatedBy;
                case ApplicationsQueryParameter.UpdatedBy:
                    return x => x.UpdatedBy;
                default:
                    return x => x.CreateTimeUtc;
            }
        }
    }
}


