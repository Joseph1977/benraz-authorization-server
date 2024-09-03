using Benraz.Infrastructure.Common.Repositories;

namespace Authorization.Domain.UsageLogs
{
    /// <summary>
    /// Usage logs repository.
    /// </summary>
    public interface IUsageLogsRepository : IRepository<int, UsageLog>
    {
    }
}


