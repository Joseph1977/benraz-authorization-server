using Benraz.Infrastructure.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authorization.Domain.ApplicationTokens
{
    /// <summary>
    /// Application tokens repository.
    /// </summary>
    public interface IApplicationTokensRepository : IRepository<Guid, ApplicationToken>
    {
        /// <summary>
        /// Returns application tokens of an application.
        /// </summary>
        /// <param name="applicationId">Application identifier.</param>
        /// <returns>Application tokens.</returns>
        Task<IEnumerable<ApplicationToken>> GetAsync(Guid applicationId);
    }
}


