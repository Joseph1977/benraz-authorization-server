using Authorization.EF;
using System.Text.Json;
using NUnit.Framework;
using Benraz.Infrastructure.Common.Http;
using System.Net.Http;
using System.Text;
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

        protected StringContent GetJsonContent(object request)
        {
            return new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        }

        protected virtual Task ClearDataAsync()
        {
            return Task.CompletedTask;
        }

        protected AuthorizationDbContext CreateDbContext()
        {
            return Config.CreateDbContext();
        }

        protected async Task<HttpResponseMessage> SendAsync(string requestUri, object request)
        {
            return await HttpClient.PostAsJsonAsync(requestUri, request);
        }

        protected async Task<HttpResponseMessage> PutAsync(string requestUri, object request)
        {
            return await HttpClient.PutAsJsonAsync(requestUri, request);
        }

        protected async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return await HttpClient.GetAsync(requestUri);
        }

        protected async Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            return await HttpClient.DeleteAsync(requestUri);
        }

        protected async Task<T> GetResponseAsync<T>(HttpResponseMessage response)
        {
            var responseContentString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseContentString);
        }
    }
}