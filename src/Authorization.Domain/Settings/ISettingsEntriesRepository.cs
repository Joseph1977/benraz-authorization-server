using Benraz.Infrastructure.Common.Repositories;

namespace Authorization.Domain.Settings
{
    /// <summary>
    /// Settings entries repository.
    /// </summary>
    public interface ISettingsEntriesRepository : IQueryRepository<string, SettingsEntry>
    {
    }
}


