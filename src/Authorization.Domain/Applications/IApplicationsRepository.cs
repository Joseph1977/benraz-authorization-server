using Benraz.Infrastructure.Common.Paging;
using Benraz.Infrastructure.Common.Repositories;
using System;
using System.Threading.Tasks;

namespace Authorization.Domain.Applications
{
    /// <summary>
    /// Applications repository.
    /// </summary>
    public interface IApplicationsRepository : IRepository<Guid, Application>
    {
        /// <summary>
        /// Returns applications page.
        /// </summary>
        /// <param name="query">Applications query.</param>
        /// <returns>Applications page.</returns>
        Task<Page<Application>> GetPageAsync(ApplicationsQuery query);
    }
}


