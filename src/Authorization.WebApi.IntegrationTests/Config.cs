using Authorization.EF;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Authorization.WebApi.IntegrationTests
{
    [SetUpFixture]
    public class Config
    {
        private static IConfiguration _configuration;

        public static AuthorizationDbContext DBContext;
        public static HttpClient HttpClient;

        [OneTimeSetUp]
        public static void SetUpFixture()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            DBContext = CreateDbContext();

            var server = new TestServer(new WebHostBuilder()
                .UseConfiguration(_configuration)
                .UseStartup<StartupStub>());

            HttpClient = server.CreateClient();

            var token = _configuration.GetValue<string>("AccessToken");
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public static AuthorizationDbContext CreateDbContext()
        {
            var dbContextOptions = new DbContextOptionsBuilder<AuthorizationDbContext>()
                .EnableSensitiveDataLogging()
                .UseSqlServer(_configuration.GetConnectionString("Authorization"))
                .Options;

            return new AuthorizationDbContext(dbContextOptions);
        }
    }
}


