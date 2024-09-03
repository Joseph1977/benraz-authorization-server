using Authorization.Domain.Applications;
using Authorization.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Authorization.EF.Tests
{
    [TestFixture]
    public class ApplicationsRepositoryTests : EFTestsBaseWithSqlServer
    {
        [SetUp]
        public override async Task SetUpAsync()
        {
            await base.SetUpAsync();
        }

        [TearDown]
        public override async Task TearDownAsync()
        {
            await base.TearDownAsync();
            await ClearDataAsync();
        }

        [Test]
        public async Task AddAsync_NewApplication_AddsApplication()
        {
            var application = new Application();
            await CreateRepository().AddAsync(application);

            var dbApplication = await CreateRepository().GetByIdAsync(application.Id);
            dbApplication.Should().BeEquivalentTo(application);
        }

        private ApplicationsRepository CreateRepository()
        {
            return new ApplicationsRepository(CreateContext());
        }

        private async Task ClearDataAsync()
        {
            var context = CreateContext();
            context.Applications.RemoveRange(await context.Applications.ToListAsync());
            await context.SaveChangesAsync();
        }
    }
}


