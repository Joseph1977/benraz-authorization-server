using Authorization.Domain.UsageLogs;

namespace Authorization.EF.Repositories
{
    /// <summary>
    /// Usage logs repository.
    /// </summary>
    public class UsageLogsRepository : AuthorizationRepositoryBase<int, UsageLog>, IUsageLogsRepository
    {
        /// <summary>
        /// Creates repository.
        /// </summary>
        /// <param name="context">Context.</param>
        public UsageLogsRepository(AuthorizationDbContext context)
            : base(context)
        {
        }
    }
}


