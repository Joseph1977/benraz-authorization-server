using System.Threading.Tasks;

namespace Authorization.EF.Services
{
    /// <summary>
    /// Database migration service.
    /// </summary>
    public interface IMigrationService
    {
        /// <summary>
        /// Migrates database.
        /// </summary>
        /// <returns>Task.</returns>
        Task MigrateAsync();
    }
}


