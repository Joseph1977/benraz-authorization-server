using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;

namespace Authorization.EF.Tests
{
    public abstract class EFTestsBase
    {
        [SetUp]
        public virtual async Task SetUpAsync()
        {
            await CreateContext().Database.MigrateAsync();
        }

        [TearDown]
        public virtual Task TearDownAsync()
        {
            return Task.CompletedTask;
        }

        protected AuthorizationDbContext CreateContext()
        {
            return new AuthorizationDbContext(CreateContextBuilder().Options);
        }

        protected abstract DbContextOptionsBuilder<AuthorizationDbContext> CreateContextBuilder();
    }

    public abstract class EFTestsBaseWithSqlServer : EFTestsBase
    {
        protected override DbContextOptionsBuilder<AuthorizationDbContext> CreateContextBuilder()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<AuthorizationDbContext>();
            builder.UseSqlServer(configuration.GetConnectionString("Authorization"));

            return builder;
        }
    }
}


