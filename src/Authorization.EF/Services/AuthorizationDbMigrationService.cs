using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Benraz.Infrastructure.Common.DataRedundancy;
using System.Threading.Tasks;

namespace Authorization.EF.Services
{
    /// <summary>
    /// Authorization database migration service.
    /// </summary>
    public class AuthorizationDbMigrationService : IMigrationService
    {
        private readonly IConfigurationRoot _configurationRoot;
        private readonly IDrChecker _drChecker;
        private readonly AuthorizationDbContext _context;

        /// <summary>
        /// Creates service.
        /// </summary>
        /// <param name="configuration">Configuration root.</param>
        /// <param name="drChecker">Data redundancy checker.</param>
        /// <param name="context">Treasury database context.</param>
        public AuthorizationDbMigrationService(
            IConfiguration configuration,
            IDrChecker drChecker,
            AuthorizationDbContext context)
        {
            _configurationRoot = (IConfigurationRoot)configuration;
            _drChecker = drChecker;
            _context = context;
        }

        /// <summary>
        /// Migrates database.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task MigrateAsync()
        {
            if (!_drChecker.IsActiveDR())
            {
                return;
            }

            await _context.Database.MigrateAsync();
            _configurationRoot.Reload();
        }
    }
}


