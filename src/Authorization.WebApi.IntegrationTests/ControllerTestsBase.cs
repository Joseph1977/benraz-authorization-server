using Authorization.EF;
using NUnit.Framework;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ErpMaintenance.WebApi.IntegrationTests
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

        protected StringContent GetJsonContent(object request)
        {
            return new StringContent(sonSerializer.Serialize(request), Encoding.UTF8, "application/json");
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


