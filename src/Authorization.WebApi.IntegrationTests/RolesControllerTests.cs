using Authorization.WebApi.Models.Roles;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Authorization.WebApi.IntegrationTests
{
    [TestFixture]
    public class RolesControllerTests : ControllerTestsBase
    {
        private IEnumerable<IdentityRole> _defaultRoles;
        private IEnumerable<IdentityRoleClaim<string>> _defaultRoleClaims;

        [SetUp]
        public override async Task SetUpAsync()
        {
            using(var dbContext = CreateDbContext())
            {
                _defaultRoles = dbContext.Roles.ToList();
                _defaultRoleClaims = dbContext.RoleClaims.ToList();
            }

            await base.SetUpAsync();
        }

        [Test]
        public async Task CanGetAllRolesAsync()
        {
            var responseContentString = await HttpClient.GetStringAsync("/v1/Roles");
            var roleViewModels = JsonSerializer.Deserialize<IEnumerable<RoleViewModel>>(responseContentString);

            roleViewModels.Should().NotBeNull();
            roleViewModels.Should().HaveCountGreaterThan(0);
        }

        [Test]
        public async Task CanGetRoleByIdAsync()
        {
            var role = await AddDefaultRoleAsync();

            var responseContentString = await HttpClient.GetStringAsync($"/v1/Roles/{role.Id}");
            var roleViewModel = JsonSerializer.Deserialize<RoleViewModel>(responseContentString);

            roleViewModel.Should().NotBeNull();
        }

        [Test]
        public async Task CanPostRoleAsync()
        {
            var dbContextRolesCount = DBContext.Roles.Count();
            var roleViewModel = new RoleViewModel
            {
                Name = "PostRole-001"
            };
            var httpContent = GetJsonContent(roleViewModel);

            var response = await HttpClient.PostAsync("/v1/Roles", httpContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContentString = await response.Content.ReadAsStringAsync();

            responseContentString.Should().NotBeNullOrEmpty();
            DBContext.Roles.Should().HaveCount(dbContextRolesCount + 1);
        }

        [Test]
        public async Task CanPutRoleAsync()
        {
            var dbContextRolesCount = DBContext.Roles.Count();
            var role = await AddDefaultRoleAsync();
            var roleViewModel = new RoleViewModel
            {
                Name = "NewRole-002"
            };
            var httpContent = GetJsonContent(roleViewModel);

            var response = await HttpClient.PutAsync($"/v1/Roles/{role.Id}", httpContent);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var responseContentString = await response.Content.ReadAsStringAsync();

            responseContentString.Should().BeNullOrEmpty();
            DBContext.Roles.Should().HaveCount(dbContextRolesCount + 1);
        }

        [Test]
        public async Task CanDeleteRoleAsync()
        {
            var dbContextRolesCount = DBContext.Roles.Count();
            var role = await AddDefaultRoleAsync();

            var response = await HttpClient.DeleteAsync($"/v1/Roles/{role.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var responseContentString = await response.Content.ReadAsStringAsync();

            responseContentString.Should().BeNullOrEmpty();
            DBContext.Roles.Should().HaveCount(dbContextRolesCount);
        }

        [Test]
        public async Task CanGetClaimsByRoleIdAsync()
        {
            var role = await AddDefaultRoleAsync();
            await AddDefaultRoleClaimsAsync(role.Id);

            var responseContentString = await HttpClient.GetStringAsync($"/v1/Roles/{role.Id}/Claims");
            var roleClaimViewModels =
                JsonSerializer.Deserialize<IEnumerable<RoleClaimViewModel>>(responseContentString);

            roleClaimViewModels.Should().NotBeNull();
            roleClaimViewModels.Should().HaveCount(2);
        }

        [Test]
        public async Task CanPutClaimsByRoleIdAsync()
        {
            var dbContextRoleClaims = DBContext.RoleClaims.Count();
            var role = await AddDefaultRoleAsync();
            await AddDefaultRoleClaimsAsync(role.Id);
            var roleClaimsViewModels = new List<RoleClaimViewModel>
            {
                new RoleClaimViewModel
                {
                    Type = "Claim",
                    Value = "ChangeClaim-001"
                },
                new RoleClaimViewModel
                {
                    Type = "Claim",
                    Value = "ChangeClaim-002"
                }, 
                new RoleClaimViewModel
                {
                    Type = "Claim",
                    Value = "NewClaim-003"
                }
            };
            var httpContent = GetJsonContent(roleClaimsViewModels);

            var response = await HttpClient.PutAsync($"/v1/Roles/{role.Id}/Claims", httpContent);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var responseContentString = await response.Content.ReadAsStringAsync();

            responseContentString.Should().BeNullOrEmpty();
            DBContext.RoleClaims.Should().HaveCount(dbContextRoleClaims + 3);
        }

        private async Task<IEnumerable<IdentityRoleClaim<string>>> AddDefaultRoleClaimsAsync(string roleId)
        {
            using(var dbContext = CreateDbContext())
            {
                var firstRoleClaim = new IdentityRoleClaim<string>
                {
                    RoleId = roleId,
                    ClaimType = "Claim",
                    ClaimValue = "ClaimValue-001"
                };
                var secondRoleClaim = new IdentityRoleClaim<string>
                {
                    RoleId = roleId,
                    ClaimType = "Claim",
                    ClaimValue = "ClaimValue-002"
                };
                dbContext.RoleClaims.Add(firstRoleClaim);
                dbContext.RoleClaims.Add(secondRoleClaim);
                await dbContext.SaveChangesAsync();

                return new List<IdentityRoleClaim<string>> { firstRoleClaim, secondRoleClaim };
            }
        }

        private async Task<IdentityRole> AddDefaultRoleAsync()
        {
            using(var dbContext = CreateDbContext())
            {
                var role = new IdentityRole
                {
                    Name = "Role-001",
                    NormalizedName = "ROLE-001"
                };
                dbContext.Roles.Add(role);
                await dbContext.SaveChangesAsync();

                return role;
            }
        }

        protected override async Task ClearDataAsync()
        {
            await base.ClearDataAsync();

            using(var dbContext = CreateDbContext())
            {
                dbContext.RoleClaims.RemoveRange(dbContext.RoleClaims);
                dbContext.Roles.RemoveRange(dbContext.Roles);

                dbContext.Roles.AddRange(_defaultRoles);
                await dbContext.SaveChangesAsync();

                dbContext.Database.OpenConnection();
                dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.RoleClaims ON");

                dbContext.RoleClaims.AddRange(_defaultRoleClaims);
                await dbContext.SaveChangesAsync();
                
                dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.RoleClaims OFF");
                dbContext.Database.CloseConnection();
            }
        }
    }
}


