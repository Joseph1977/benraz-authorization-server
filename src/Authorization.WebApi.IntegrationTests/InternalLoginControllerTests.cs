using Authorization.Domain.SsoConnections;
using Authorization.WebApi.Models.Addresses;
using Authorization.WebApi.Models.InternalLogin;
using Authorization.WebApi.Models.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using NUnit.Framework;
using Benraz.Infrastructure.Domain.Authorization;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Authorization.WebApi.IntegrationTests
{
    [TestFixture]
    public class InternalLoginControllerTests : ControllerTestsBase
    {
        [SetUp]
        public override async Task SetUpAsync()
        {
            await base.SetUpAsync();
        }

        [Test]
#if !DEBUG
        [Ignore("Ignore in production.")]
#endif
        public async Task CanSignUpAsync()
        {
            var signUpViewModel = SignUpViewModel();

            var response = await SendAsync($"/v1/internal-login/sign-up", signUpViewModel);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var viewModel = await GetResponseAsync<UserOpenIdViewModel>(response);
            viewModel.Email.Should().Be(signUpViewModel.Username);
        }

        [Test]
        public async Task CanProcessInternalLoginAsync()
        {
            await AddDefaultUserAsync();
            await VerifyDefaultUserEmailAsync();
            var application = await AddDefaultApplicationAsync();
            await AddDefaultSsoConnectionAsync(application.Id);

            var internalLoginViewModel = new LoginViewModel
            {
                State = $"applicationId={application.Id}",
                Username = "test@test.org",
                Password = "qW_12356"
            };
            var httpContent = GetJsonContent(internalLoginViewModel);

            var response = await HttpClient.PostAsync($"/v1/internal-login", httpContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContentString = await response.Content.ReadAsStringAsync();
            var internalLoginResultViewModel =
                JsonSerializer.Deserialize<LoginResultViewModel>(responseContentString);
            internalLoginResultViewModel.CallbackUrl.Should().NotBeNullOrEmpty();
        }

        private async Task AddDefaultUserAsync()
        {
            var userViewModel = new CreateUserViewModel
            {
                FullName = "PostUserName-001",
                Email = "test@test.org",
                Password = "qW_12356"
            };
            var httpContent = GetJsonContent(userViewModel);

            await HttpClient.PostAsync("/v1/Users", httpContent);
        }

        private async Task VerifyDefaultUserEmailAsync()
        {
            using (var dbContext = CreateDbContext())
            {
                var user = await dbContext.Users.FirstOrDefaultAsync();
                user.EmailConfirmed = true;

                dbContext.Update(user);
                await dbContext.SaveChangesAsync();
            }
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

        private SignUpViewModel SignUpViewModel()
        {
            return new SignUpViewModel
            {
                Profile = "Profile test",
                ZoneInfo = "Zone info test",
                PhoneNumber = "0546546548",
                FirstName = "First name test",
                LastName = "Last name test",
                NickName = "Nickname test",
                SendConfirmationEmail = true,
                Password = "1q2w3e$R",
                Username = "Test@test.com",
                Address = new AddressViewModel
                {
                    Country = "Country test",
                    Locality = "Locality  test",
                    PostalCode = "Post code test",
                    Region = "Region test",
                    StreetAddress = "Address test"
                }
            };
        }
    }
}