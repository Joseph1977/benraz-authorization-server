using Authorization.Domain.UsageLogs;
using Authorization.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Authorization.EF.Tests
{
    [TestFixture]
    public class UsageLogsRepositoryTests : EFTestsBaseWithSqlServer
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
        public async Task AddAsync_NewEntity_AddsEntity()
        {
            var entity = new UsageLog
            {
                UserName = "UserName001",
                IPAddress = "127.0.0.1",
                Action = "Action001"
            };
            await CreateRepository().AddAsync(entity);

            var dbEntity = await CreateRepository().GetByIdAsync(entity.Id);
            dbEntity.Should().BeEquivalentTo(entity);
        }

        private UsageLogsRepository CreateRepository()
        {
            return new UsageLogsRepository(CreateContext());
        }

        private async Task ClearDataAsync()
        {
            var context = CreateContext();
            context.UsageLogs.RemoveRange(await context.UsageLogs.ToListAsync());
            await context.SaveChangesAsync();
        }
    }
}


