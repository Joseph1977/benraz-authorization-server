using Authorization.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Benraz.Infrastructure.Domain.Authorization;

namespace Authorization.EF.DataConfigurations
{
    class UserStatusDataConfiguration : IEntityTypeConfiguration<UserStatus>
    {
        public void Configure(EntityTypeBuilder<UserStatus> builder)
        {
            builder.HasData(
                Create(UserStatusCode.Active, "Active"),
                Create(UserStatusCode.Suspended, "Suspended"),
                Create(UserStatusCode.Blocked, "Blocked"),
                Create(UserStatusCode.PaymentServiceSuspended, "Payment service suspended"));
        }

        private UserStatus Create(UserStatusCode id, string name)
        {
            return new UserStatus
            {
                Id = id,
                Name = name
            };
        }
    }
}


