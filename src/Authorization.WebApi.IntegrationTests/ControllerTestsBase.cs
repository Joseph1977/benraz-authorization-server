using Authorization.EF;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace Authorization.WebApi.IntegrationTests
{
    [TestFixture]
    public abstract class ControllerTestsBase
    {
        protected HttpClient HttpClient;
        protected AuthorizationDbContext DBContext;

        [SetUp]
        public virtual async Task SetUpAsync()
        {
            HttpClient = Config.HttpClient;
            DBContext = Config.DBContext;

            await ClearDataAsync();
        }

        [TearDown]
        public virtual async Task TearDownAsync()
        {
            await ClearDataAsync();
        }

        protected virtual Task ClearDataAsync()
        {
            return Task.CompletedTask;
        }

        protected AuthorizationDbContext CreateDbContext()
        {
            return Config.CreateDbContext();
        }
    }
}

