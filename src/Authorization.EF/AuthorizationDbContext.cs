using Authorization.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Authorization.EF
{
    /// <summary>
    /// Database context.
    /// </summary>
    public class AuthorizationDbContext : DbContext
    {
        /// <summary>
        /// Settings entries.
        /// </summary>
        public DbSet<SettingsEntry> SettingsEntries { get; set; }

        /// <summary>
        /// Creates context.
        /// </summary>
        /// <param name="options">Context options.</param>
        public AuthorizationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetAssembly(typeof(AuthorizationDbContext)));
        }
    }
}

