using Authorization.Domain.Settings;
using Benraz.Infrastructure.EF;

namespace Authorization.EF.Repositories
{
    /// <summary>
    /// Settings entries repository.
    /// </summary>
    public class SettingsEntriesRepository :
        QueryRepositoryBase<string, SettingsEntry, AuthorizationDbContext>,
        ISettingsEntriesRepository
    {
        /// <summary>
        /// Creates repository.
        /// </summary>
        /// <param name="context">Context.</param>
        public SettingsEntriesRepository(AuthorizationDbContext context)
            : base(context)
        {
        }
    }
}


