using Authorization.Domain.ApplicationTokens;
using Authorization.Domain.SsoConnections;
using Authorization.WebApi.Models.Auth;
using Authorization.WebApi.Models.Users;
using FluentAssertions;
using NUnit.Framework;
using Benraz.Infrastructure.Domain.Authorization;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Authorization.WebApi.IntegrationTests
{
    [TestFixture]
    public class AuthControllerTests : ControllerTestsBase
    {
        [SetUp]
        public override async Task SetUpAsync()
        {
            await base.SetUpAsync();
        }

        [Test]
        public async Task CanLoginUserAsync()
        {
            var application = await AddDefaultApplicationAsync();
            await AddDefaultSsoConnectionAsync(application.Id);
            await AddDefaultUserAsync();
            var loginViewModel = new LoginViewModel
            {
                ApplicationId = application.Id,
                SsoProviderCode = SsoProviderCode.Microsoft
            };
            var queryString =
                $"?applicationId={loginViewModel.ApplicationId}" +
                $"&ssoProviderCode={loginViewModel.SsoProviderCode}";

            var response = await HttpClient.GetAsync($"/v1/Auth/login{queryString}");
            response.StatusCode.Should().Be(HttpStatusCode.Redirect);
            response.Headers.Location.Should().NotBeNull();

            var responseContentString = await response.Content.ReadAsStringAsync();
            responseContentString.Should().BeNullOrEmpty();
        }

        [Test]
        public async Task CanGetPublicKeysAsync()
        {
            var response = await HttpClient.GetAsync("/v1/Auth/keys");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContentString = await response.Content.ReadAsStringAsync();
            responseContentString.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task CanGetAuthParametersAsync()
        {
            var response = await HttpClient.GetAsync("/v1/Auth/parameters");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContentString = await response.Content.ReadAsStringAsync();
            responseContentString.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task CanCheckIfTokenIsActiveAsync()
        {
            var application = await AddDefaultApplicationAsync();
            var applicationToken = await AddDefaultApplicationTokenAsync(application.Id);

            var response = await HttpClient.GetAsync($"/v1/Auth/token/{applicationToken.Id}/is-active");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContentString = await response.Content.ReadAsStringAsync();
            responseContentString.Should().BeEquivalentTo("true");
        }

        private async Task AddDefaultUserAsync()
        {
            var userViewModel = new CreateUserViewModel
            {
                FullName = "PostUserName-001",
                Email = "test@test.org",
                Password = "qW_123"
            };
            var httpContent = GetJsonContent(userViewModel);

            await HttpClient.PostAsync("/v1/Users", httpContent);
        }

        private async Task<SsoConnection> AddDefaultSsoConnectionAsync(Guid applicationId)
        {
            using (var dbContext = CreateDbContext())
            {
                var ssoConnection = new SsoConnection
                {
                    ApplicationId = applicationId,
                    SsoProviderCode = SsoProviderCode.Internal,
                    IsEnabled = true,
                };
                dbContext.SsoConnections.Add(ssoConnection);
                await dbContext.SaveChangesAsync();

                return ssoConnection;
            }
        }

        private async Task<Domain.Applications.Application> AddDefaultApplicationAsync()
        {
            using (var dbContext = CreateDbContext())
            {
                var application = new Domain.Applications.Application
                {
                    Name = "Application-001",
                    Audience = "Audience-002"
                };
                dbContext.Applications.Add(application);
                await dbContext.SaveChangesAsync();

                return application;
            }
        }

        private async Task<ApplicationToken> AddDefaultApplicationTokenAsync(Guid applicationId)
        {
            using (var dbContext = CreateDbContext())
            {
                var applicationToken = new ApplicationToken
                {
                    ApplicationId = applicationId,
                    Name = "ApplicationToken-001",
                    Value = "ApplicationTokenValue",
                    ExpirationTimeUtc = DateTime.UtcNow.AddDays(1)
                };
                dbContext.ApplicationTokens.Add(applicationToken);
                await dbContext.SaveChangesAsync();

                return applicationToken;
            }
        }

        protected override async Task ClearDataAsync()
        {
            await base.ClearDataAsync();

            using (var dbContext = CreateDbContext())
            {
                dbContext.ApplicationTokens.RemoveRange(dbContext.ApplicationTokens);
                dbContext.Applications.RemoveRange(dbContext.Applications);
                dbContext.Users.RemoveRange(dbContext.Users);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}