using Authorization.Domain.Applications;
using Authorization.Domain.ApplicationTokens;
using Authorization.Domain.Claims;
using Authorization.Domain.Settings;
using Authorization.Domain.SsoConnections;
using Authorization.Domain.SsoProviders;
using Authorization.Domain.UsageLogs;
using Authorization.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Authorization.EF
{
    /// <summary>
    /// Authorization database context.
    /// </summary>
    public class AuthorizationDbContext : IdentityDbContext<
        User, 
        IdentityRole, 
        string, 
        IdentityUserClaim<string>, 
        UserRole, 
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
        /// Applications.
        /// </summary>
        public DbSet<Application> Applications { get; set; }

        /// <summary>
        /// Application URLs.
        /// </summary>
        public DbSet<ApplicationUrl> ApplicationUrls { get; set; }

        /// <summary>
        /// Application URL types.
        /// </summary>
        public DbSet<ApplicationUrlType> ApplicationUrlTypes { get; set; }

        /// <summary>
        /// Application tokens.
        /// </summary>
        public DbSet<ApplicationToken> ApplicationTokens { get; set; }

        /// <summary>
        /// SSO providers.
        /// </summary>
        public DbSet<SsoProvider> SsoProviders { get; set; }

        /// <summary>
        /// SSO connections.
        /// </summary>
        public DbSet<SsoConnection> SsoConnections { get; set; }

        /// <summary>
        /// Claims.
        /// </summary>
        public DbSet<IdentityClaim> Claims { get; set; }

        /// <summary>
        /// User passwords.
        /// </summary>
        public DbSet<UserPassword> UserPasswords { get; set; }

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
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(AuthorizationDbContext)));
        }
    }
}


