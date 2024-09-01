using Authorization.Domain.Applications;
using Authorization.Domain.ApplicationTokens;
using Authorization.WebApi.Models.Applications;
using ErpMaintenance.WebApi.IntegrationTests;
using FluentAssertions;
using System.Text.Json;
using NUnit.Framework;
using Benraz.Infrastructure.Common.Paging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.WebApi.IntegrationTests
{
    [TestFixture]
    public class ApplicationsControllerTests : ControllerTestsBase
    {
        [SetUp]
        public override async Task SetUpAsync()
        {
            await base.SetUpAsync();
        }

        [Test]
        public async Task CanGetApplicationsByQueryAsync()
        {
            await AddDefaultApplicationAsync();
            await AddDefaultApplicationAsync();

            var applicationsQuery = new ApplicationsQuery
            {
                SortBy = ApplicationsQueryParameter.Id,
                SortDesc = false,
                PageNo = 1,
                PageSize = 20
            };
            var queryString =
                $"?sortBy={applicationsQuery.SortBy}&sortDesc={applicationsQuery.SortDesc}" +
                $"&pageNo={applicationsQuery.PageNo}&pageSize={applicationsQuery.PageSize}";

            var responseContentString = await HttpClient.GetStringAsync($"/v1/Applications{queryString}");
            var applicationViewModels =
                JsonSerializer.Deserialize<Page<ApplicationViewModel>>(responseContentString);

            applicationViewModels.Should().NotBeNull();
            applicationViewModels.Items.Should().HaveCount(2);
        }

        [Test]
        public async Task CanGetApplicationByIdAsync()
        {
            var application = await AddDefaultApplicationAsync();
            
            var responseContentString = await HttpClient.GetStringAsync($"/v1/Applications/{application.Id}");
            var applicationViewModel = JsonSerializer.Deserialize<ApplicationViewModel>(responseContentString);

            applicationViewModel.Should().NotBeNull();
        }

        [Test]
        public async Task CanPostApplicationAsync()
        {
            var applicationViewModel = new ApplicationViewModel
            {
                Name = "PostApplication-001",
                Audience = "Audience-001"
            };
            var httpContent = GetJsonContent(applicationViewModel);

            var response = await HttpClient.PostAsync("/v1/Applications", httpContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContentString = await response.Content.ReadAsStringAsync();

            responseContentString.Should().NotBeNullOrEmpty();
            DBContext.Applications.Should().HaveCount(1);
        }

        [Test]
        public async Task CanPutApplicationAsync()
        {
            var application = await AddDefaultApplicationAsync();
            var applicationViewModel = new ApplicationViewModel
            {
                Name = "NewApplicationName",
                Audience = "NewAudience"
            };
            var httpContent = new StringContent(
                sonSerializer.Serialize(applicationViewModel), Encoding.UTF8, "application/json");

            var response = await HttpClient.PutAsync($"/v1/Applications/{application.Id}", httpContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContentString = await response.Content.ReadAsStringAsync();
            responseContentString.Should().NotBeNullOrEmpty();
            DBContext.Applications.Should().HaveCount(1);
        }

        [Test]
        public async Task CanDeleteApplicationAsync()
        {
            var application = await AddDefaultApplicationAsync();
            var response = await HttpClient.DeleteAsync($"/v1/Applications/{application.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var responseContentString = await response.Content.ReadAsStringAsync();
            responseContentString.Should().BeNullOrEmpty();
            DBContext.Applications.Should().HaveCount(0);
        }

        [Test]
        public async Task CanGetApplicationTokensByApplicationIdAsync()
        {
            var application = await AddDefaultApplicationAsync();
            await AddDefaultApplicationTokenAsync(application.Id);
            await AddDefaultApplicationTokenAsync(application.Id);

            var responseContentString = await HttpClient.GetStringAsync($"/v1/Applications/{application.Id}/tokens");
            var applicationTokenViewModels =
                JsonSerializer.Deserialize<IEnumerable<ApplicationTokenViewModel>>(responseContentString);

            applicationTokenViewModels.Should().NotBeNull();
            applicationTokenViewModels.Should().HaveCount(2);
        }

        [Test]
        public async Task CanPostApplicationTokenAsync()
        {
            var application = await AddDefaultApplicationAsync();
            var applicationTokenViewModel = new CreateApplicationTokenViewModel
            {
                Name = "PostApplicationToken-001",
                Roles = new List<string> { "Role-001" },
                Claims = new List<ApplicationTokenClaimViewModel>
                {
                    new ApplicationTokenClaimViewModel { Type = "Claim", Value = "Claim-001" }
                },
                ExpirationTimeUtc = DateTime.UtcNow.AddDays(1)
            };
            var httpContent = GetJsonContent(applicationTokenViewModel);

            var response = await HttpClient.PostAsync($"/v1/Applications/{application.Id}/tokens", httpContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContentString = await response.Content.ReadAsStringAsync();

            responseContentString.Should().NotBeNullOrEmpty();
            DBContext.Applications.Should().HaveCount(1);
            DBContext.ApplicationTokens.Should().HaveCount(1);
        }

        [Test]
        public async Task CanDeleteApplicationTokenAsync()
        {
            var application = await AddDefaultApplicationAsync();
            var applicationToken = await AddDefaultApplicationTokenAsync(application.Id);

            var response =
                await HttpClient.DeleteAsync($"/v1/Applications/{application.Id}/tokens/{applicationToken.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var responseContentString = await response.Content.ReadAsStringAsync();
            responseContentString.Should().BeNullOrEmpty();
            DBContext.ApplicationTokens.Should().HaveCount(0);
        }

        private async Task<ApplicationToken> AddDefaultApplicationTokenAsync(Guid applicationId)
        {
            using (var dbContext = CreateDbContext())
            {
                var applicationToken = new ApplicationToken
                {
                    ApplicationId = applicationId,
                    Name = "ApplicationToken-001",
                    Value = "ApplicationTokenValue"
                };
                dbContext.ApplicationTokens.Add(applicationToken);
                await dbContext.SaveChangesAsync();

                return applicationToken;
            }
        }

        private async Task<Application> AddDefaultApplicationAsync()
        {
            using (var dbContext = CreateDbContext())
            {
                var application = new Application
                {
                    Name = "Application-001",
                    Audience = "Audience-002"
                };
                dbContext.Applications.Add(application);
                await dbContext.SaveChangesAsync();

                return application;
            }
        }

        protected override async Task ClearDataAsync()
        {
            await base.ClearDataAsync();

            using (var dbContext = CreateDbContext())
            {
                dbContext.ApplicationTokens.RemoveRange(dbContext.ApplicationTokens);
                dbContext.Applications.RemoveRange(dbContext.Applications);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}


