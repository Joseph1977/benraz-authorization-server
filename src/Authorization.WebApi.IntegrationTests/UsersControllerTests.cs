using Authorization.Domain.Users;
using Authorization.WebApi.Models.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using NUnit.Framework;
using Benraz.Infrastructure.Common.Paging;
using Benraz.Infrastructure.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Authorization.WebApi.IntegrationTests
{
    [TestFixture]
    public class UsersControllerTests : ControllerTestsBase
    {
        [SetUp]
        public override async Task SetUpAsync()
        {
            await base.SetUpAsync();
        }

        [Test]
        public async Task CanGetUsersByQueryAsync()
        {
            await AddDefaultUsersAsync();
            var usersQuery = new UsersQuery
            {
                SortBy = UsersQueryParameter.PhoneNumber,
                SortDesc = false,
                PageNo = 1,
                PageSize = 10
            };
            var queryString =
                $"?sortBy={usersQuery.SortBy}&sortDesc={usersQuery.SortDesc}" +
                $"&pageNo={usersQuery.PageNo}&pageSize={usersQuery.PageSize}";

            var responseContentString = await HttpClient.GetStringAsync($"/v1/Users/{queryString}");
            var userViewModels = JsonSerializer.Deserialize<Page<UserInfoViewModel>>(responseContentString);

            userViewModels.Should().NotBeNull();
            userViewModels.Items.Should().HaveCount(2);
        }

        [Test]
        public async Task CanGetUserByIdAsync()
        {
            var users = await AddDefaultUsersAsync();
            var user = users.ToList()[0];

            var responseContentString = await HttpClient.GetStringAsync($"/v1/Users/{user.Id}");
            var userViewModel = JsonSerializer.Deserialize<UserInfoViewModel>(responseContentString);

            userViewModel.Should().NotBeNull();
        }

        [Test]
        public async Task CanPostUserAsync()
        {
            var userViewModel = new CreateUserViewModel
            {
                FullName = "PostUserName-001",
                Email = "test@test.org",
                Password = "Aa12345_"
            };
            var httpContent = GetJsonContent(userViewModel);

            var response = await HttpClient.PostAsync("/v1/Users", httpContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContentString = await response.Content.ReadAsStringAsync();

            responseContentString.Should().NotBeNullOrEmpty();
            DBContext.Users.Should().HaveCount(1);
        }

        [Test]
        public async Task CanPutUserAsync()
        {
            var users = await AddDefaultUsersAsync();
            var user = users.ToList()[0];

            var userViewModel = new ChangeUserViewModel
            {
                FullName = "UserName002"
            };
            var httpContent = GetJsonContent(userViewModel);

            var response = await HttpClient.PutAsync($"/v1/Users/{user.Id}", httpContent);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var retrievedUser = await DBContext.Users.FindAsync(user.Id);

            // Assert: Verify the result
            retrievedUser.Should().NotBeNull("because a user with the given ID should exist in the database");
            retrievedUser.FullName.Should().Be("UserName002");
        }

        [Test]
        public async Task CanDeleteUserAsync()
        {
            var users = await AddDefaultUsersAsync();
            var user = users.ToList()[0];

            var response = await HttpClient.DeleteAsync($"/v1/Users/{user.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var responseContentString = await response.Content.ReadAsStringAsync();
            responseContentString.Should().BeNullOrEmpty();
            DBContext.Users.Should().HaveCount(1);
        }

        [Test]
        public async Task CanGetUserStatusAsync()
        {
            var users = await AddDefaultUsersAsync();
            var user = users.ToList()[0];

            var response = await HttpClient.GetAsync($"/v1/Users/{user.Id}/status");
            var responseContentString = await response.Content.ReadAsStringAsync();
            var userStatusCode = JsonSerializer.Deserialize<UserStatusCode>(responseContentString);

            userStatusCode.Should().NotBe(default(UserStatusCode), "because the user status code should be a valid enum value and not the default.");

        }

        [Test]
        public async Task CanChangeUserPaymentSuspensionStatusAsync()
        {
            var users = await AddDefaultUsersAsync();
            var user = users.ToList()[0];
            var httpContent = GetJsonContent(false);

            var response = await HttpClient.PostAsync($"/v1/Users/{user.Id}/suspend-payment", httpContent);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var responseContentString = await response.Content.ReadAsStringAsync();
            responseContentString.Should().BeNullOrEmpty();
            var retrievedUser = await DBContext.Users.FindAsync(user.Id);

            // Assert: Verify the result
            retrievedUser.Should().NotBeNull("because a user with the given ID should exist in the database");
            retrievedUser.StatusCode.Should().Be(UserStatusCode.Active, "because the user's status code should be 1");

        }

        [Test]
        public async Task CanChangeUserBlockStatusAsync()
        {
            var users = await AddDefaultUsersAsync();
            var user = users.ToList()[1];
            var httpContent = GetJsonContent(true);

            var response = await HttpClient.PostAsync($"/v1/Users/{user.Id}/block", httpContent);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var responseContentString = await response.Content.ReadAsStringAsync();
            responseContentString.Should().BeNullOrEmpty();
            var retrievedUser = await DBContext.Users.FindAsync(user.Id);

            // Assert: Verify the result
            retrievedUser.Should().NotBeNull("because a user with the given ID should exist in the database");
            retrievedUser.StatusCode.Should().Be(UserStatusCode.Blocked, "because the user's status code should be 3");
        }

        [Test]
        public async Task CanGetUserRolesByUserIdAsync()
        {
            var users = await AddDefaultUsersAsync();
            var user = users.ToList()[0];
            await AddDefaultUserRolesAsync(user.Id);

            var response = await HttpClient.GetAsync($"/v1/Users/{user.Id}/roles");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContentString = await response.Content.ReadAsStringAsync();
            responseContentString.Should().NotBeNullOrEmpty();

            var dbUserRoles = JsonSerializer.Deserialize<List<string>>(responseContentString);
            dbUserRoles.Should().HaveCount(2);
        }

        [Test]
        public async Task CanUpdateUserRolesAsync()
        {
            var users = await AddDefaultUsersAsync();
            var user = users.ToList()[0];
            await AddDefaultUserRolesAsync(user.Id);

            var newUserRoles = new List<string>
            {
                "Tenant",
                "Property manager"
            };
            var httpContent = GetJsonContent(newUserRoles);

            var response = await HttpClient.PutAsync($"/v1/Users/{user.Id}/roles", httpContent);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var responseContentString = await response.Content.ReadAsStringAsync();
            responseContentString.Should().BeNullOrEmpty();
        }

        [Test]
        public async Task CanGetUserClaimsByUserIdAsync()
        {
            var users = await AddDefaultUsersAsync();
            var user = users.ToList()[0];
            await AddDefaultUserClaimsAsync(user.Id);

            var response = await HttpClient.GetAsync($"/v1/Users/{user.Id}/claims");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContentString = await response.Content.ReadAsStringAsync();
            responseContentString.Should().NotBeNullOrEmpty();

            var dbUserClaims = JsonSerializer.Deserialize<List<object>>(responseContentString);
            dbUserClaims.Should().HaveCount(2);
        }

        [Test]
        public async Task CanUpdateUserClaimsAsync()
        {
            var users = await AddDefaultUsersAsync();
            var user = users.ToList()[0];
            await AddDefaultUserClaimsAsync(user.Id);

            var newUserClaims = new List<UserClaimViewModel>
            {
                new UserClaimViewModel
                {
                    Type = "Claim",
                    Value = "authorization-user-claim-update"
                },
                new UserClaimViewModel
                {
                    Type = "Claim",
                    Value = "authorization-application-read"
                }
            };
            var httpContent = GetJsonContent(newUserClaims);

            var response = await HttpClient.PutAsync($"/v1/Users/{user.Id}/claims", httpContent);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var responseContentString = await response.Content.ReadAsStringAsync();
            responseContentString.Should().BeNullOrEmpty();
        }

        [Test]
        public async Task CanGetUserEmailByIdAsync()
        {
            var users = await AddDefaultUsersAsync();
            var user = users.ToList()[0];

            var response = await HttpClient.GetAsync($"/v1/Users/{user.Id}/email");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContentString = await response.Content.ReadAsStringAsync();
            responseContentString.Should().BeEquivalentTo("User@Email-001");
        }

        [Test]
        public async Task CanConfirmUserEmailByIdAsync()
        {
            var users = await AddDefaultUsersAsync();
            var user = users.ToList()[0];
            var httpContent = GetJsonContent("");

            var response = await HttpClient.PostAsync($"/v1/Users/{user.Id}/verify-email", httpContent);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var responseContentString = await response.Content.ReadAsStringAsync();
            responseContentString.Should().BeNullOrEmpty();

            var retrievedUser = await DBContext.Users.FindAsync(user.Id);

            // Assert: Verify the result
            retrievedUser.Should().NotBeNull("because a user with the given ID should exist in the database");
            retrievedUser.EmailConfirmed.Should().BeTrue();
        }

        [Test]
        public async Task CanGetUserPhoneNumberByIdAsync()
        {
            var users = await AddDefaultUsersAsync();
            var user = users.ToList()[0];

            var response = await HttpClient.GetAsync($"/v1/Users/{user.Id}/phone");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContentString = await response.Content.ReadAsStringAsync();
            responseContentString.Should().BeEquivalentTo("222-222");
        }

        [Test]
        public async Task CanConfirmUserPhoneNumberByIdAsync()
        {
            var users = await AddDefaultUsersAsync();
            var user = users.ToList()[0];
            var httpContent = GetJsonContent("");

            var response = await HttpClient.PostAsync($"/v1/Users/{user.Id}/verify-phone", httpContent);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var responseContentString = await response.Content.ReadAsStringAsync();
            responseContentString.Should().BeNullOrEmpty();

            var retrievedUser = await DBContext.Users.FindAsync(user.Id);

            // Assert: Verify the result
            retrievedUser.Should().NotBeNull("because a user with the given ID should exist in the database");
            retrievedUser.PhoneNumberConfirmed.Should().BeTrue();
        }

        [Test]
        public async Task CanResetUserPasswordToNewOneAsync()
        {
            var users = await AddDefaultUsersAsync();
            var user = users.ToList()[0];
            var httpContent = GetJsonContent("aA_12345");

            var response = await HttpClient.PostAsync($"/v1/Users/{user.Id}/password", httpContent);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var responseContentString = await response.Content.ReadAsStringAsync();
            responseContentString.Should().BeNullOrEmpty();
        }

        [Test]
        public async Task CanUnlockUserAsync()
        {
            var users = await AddDefaultUsersAsync();
            var user = users.ToList()[1];

            var response = await HttpClient.PostAsync($"/v1/Users/{user.Id}/unlock", null);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var responseContentString = await response.Content.ReadAsStringAsync();
            responseContentString.Should().BeNullOrEmpty();

            var retrievedUser = await DBContext.Users.FindAsync(user.Id);

            // Assert: Verify the result
            retrievedUser.Should().NotBeNull("because a user with the given ID should exist in the database");
            retrievedUser.LockoutEnd.Should().BeNull();
            retrievedUser = await DBContext.Users.FindAsync(user.Id);
            retrievedUser.AccessFailedCount.Should().Be(0);
        }

        [Test]
        public async Task CanGetUserInfoByIdAsync()
        {
            var users = await AddDefaultUsersAsync();
            var user = users.ToList()[0];

            var response = await HttpClient.GetAsync($"/v1/Users/{user.Id}/userInfoById");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var viewModel = await GetResponseAsync<UserOpenIdViewModel>(response);
            viewModel.Email.Should().BeEquivalentTo("User@Email-001");
        }

        [Test]
        public async Task CanGetUserInfoByEmailAsync()
        {
            var users = await AddDefaultUsersAsync();
            var user = users.ToList()[0];

            var response = await HttpClient.GetAsync($"/v1/Users/userInfoByEmail?email={user.Email}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var viewModel = await GetResponseAsync<UserOpenIdViewModel>(response);
            viewModel.Email.Should().BeEquivalentTo("User@Email-001");
        }

        private async Task<IEnumerable<IdentityUserClaim<string>>> AddDefaultUserClaimsAsync(string userId)
        {
            using(var dbContext = CreateDbContext())
            {
                var userClaims = new List<IdentityUserClaim<string>>
                {
                    new IdentityUserClaim<string>
                    {
                        UserId = userId,
                        ClaimType = "Claim",
                        ClaimValue = "authorization-user-update"
                    },
                    new IdentityUserClaim<string>
                    {
                        UserId = userId,
                        ClaimType = "Claim",
                        ClaimValue = "erpmaintenance-client-delete"
                    }
                };

                DBContext.UserClaims.AddRange(userClaims);
                await DBContext.SaveChangesAsync();

                return userClaims;
            }
        }

        private async Task<IEnumerable<IdentityUserRole<string>>> AddDefaultUserRolesAsync(string userId)
        {
            using(var dbContext = CreateDbContext())
            {
                var userRoles = new List<UserRole>
                {
                    new UserRole
                    {
                        UserId = userId,
                        RoleId = "25c5290f-91bc-4c47-ad4f-9fc01f6191cd"
                    },
                    new UserRole
                    {
                        UserId = userId,
                        RoleId = "d14a3723-2ca1-49b2-9d58-50220836053d"
                    }
                };
                DBContext.UserRoles.AddRange(userRoles);
                await DBContext.SaveChangesAsync();

                return userRoles;
            }
        }

        private async Task<IEnumerable<User>> AddDefaultUsersAsync()
        {
            using (var dbContext = CreateDbContext())
            {
                var users = new List<User>
                {
                    new User
                    {
                        UserName = "UserName-001",
                        FullName = "User name",
                        Email = "User@Email-001",
                        PhoneNumber = "222-222",
                        StatusCode = UserStatusCode.PaymentServiceSuspended,
                        AccessFailedCount = 1
                    },
                    new User
                    {
                        UserName = "UserName-002",
                        FullName = "User name",
                        Email = "User@Email-002",
                        PhoneNumber = "001-001",
                        StatusCode = UserStatusCode.Active,
                        AccessFailedCount = 1,
                        LockoutEnd = new DateTimeOffset(DateTime.Now)
                    }
                };
                dbContext.Users.AddRange(users);
                await dbContext.SaveChangesAsync();

                return users;
            }
        }

        protected override async Task ClearDataAsync()
        {
            await base.ClearDataAsync();

            using (var dbContext = CreateDbContext())
            {
                dbContext.Users.RemoveRange(dbContext.Users);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}


