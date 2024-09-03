using Authorization.Domain.Claims;
using Authorization.WebApi.Models.Claims;
using FluentAssertions;
using System.Text.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Authorization.WebApi.IntegrationTests
{
    [TestFixture]
    public class ClaimsControllerTests : ControllerTestsBase
    {
        private IEnumerable<IdentityClaim> _defaultClaims;

        [SetUp]
        public override async Task SetUpAsync()
        {
            _defaultClaims = CreateDbContext().Claims.ToList();
            await base.SetUpAsync();
        }

        [Test]
        public async Task CanGetAllClaimsAsync()
        {
            var dbContextClaimsCount = DBContext.Claims.Count();

            var responseContentString = await HttpClient.GetStringAsync("/v1/Claims");
            var claimViewModels = JsonSerializer.Deserialize<IEnumerable<ClaimViewModel>>(responseContentString);

            claimViewModels.Should().NotBeNull();
            claimViewModels.Should().HaveCount(dbContextClaimsCount);
        }

        [Test]
        public async Task CanPostClaimAsync()
        {
            var dbContextClaimsCount = DBContext.Claims.Count();
            var claimViewModel = new ClaimViewModel
            {
                Type = "Claim-001",
                Value = "ClaimValue-001"
            };
            var httpContent = GetJsonContent(claimViewModel);

            var response = await HttpClient.PostAsync("/v1/Claims", httpContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContentString = await response.Content.ReadAsStringAsync();

            responseContentString.Should().NotBeNullOrEmpty();
            DBContext.Claims.Should().HaveCount(dbContextClaimsCount + 1);
        }

        [Test]
        public async Task CanDeleteClaimAsync()
        {
            var dbContextClaimsCount = DBContext.Claims.Count();
            var claim = await AddDefaultClaimAsync();
            var response = await HttpClient.DeleteAsync($"/v1/Claims/{claim.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var responseContentString = await response.Content.ReadAsStringAsync();
            responseContentString.Should().BeNullOrEmpty();
            DBContext.Claims.Should().HaveCount(dbContextClaimsCount);
        }

        private async Task<IdentityClaim> AddDefaultClaimAsync()
        {
            using (var dbContext = CreateDbContext())
            {
                var claim = new IdentityClaim
                {
                    Type = "Claim-001",
                    Value = "ClaimValue-001"
                };
                dbContext.Claims.Add(claim);
                await dbContext.SaveChangesAsync();

                return claim;
            }
        }

        protected override async Task ClearDataAsync()
        {
            await base.ClearDataAsync();

            using (var dbContext = CreateDbContext())
            {
                dbContext.Claims.RemoveRange(dbContext.Claims);
                dbContext.Claims.AddRange(_defaultClaims);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}


