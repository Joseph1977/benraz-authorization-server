using System.Threading.Tasks;

namespace Authorization.EF.Services
{
    /// <summary>
    /// Database migration service.
    /// </summary>
    public interface IDbMigrationService
    {
        /// <summary>
        /// Migrates database.
        /// </summary>
        /// <returns>Task.</returns>
        Task MigrateAsync();
    }
}

