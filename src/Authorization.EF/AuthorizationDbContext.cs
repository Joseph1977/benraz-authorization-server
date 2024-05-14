using Authorization.Domain.Claims;
using Authorization.Domain.Settings;
using Authorization.Domain.UsageLogs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Authorization.EF
{
    /// <summary>
    /// Database context.
    /// </summary>
    public class AuthorizationDbContext : IdentityDbContext<
    //User,
    IdentityRole,
    string,
    IdentityUserClaim<string>,
    //UserRole,
    IdentityUserLogin<string>,
    IdentityRoleClaim<string>,
    IdentityUserToken<string>>
    {
        /// <summary>
        /// Settings entries.
        /// </summary>
        public DbSet<SettingsEntry> SettingsEntries { get; set; }

        /// <summary>
        /// Usage logs.
        /// </summary>
        public DbSet<UsageLog> UsageLogs { get; set; }

        /// <summary>
        /// Claims.
        /// </summary>
        public DbSet<IdentityClaim> Claims { get; set; }


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

