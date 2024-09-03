using Authorization.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.EF.Configurations
{
    class UserStatusConfiguration : IEntityTypeConfiguration<UserStatus>
    {
        public void Configure(EntityTypeBuilder<UserStatus> builder)
        {
            builder.ToTable("UserStatuses");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("UserStatusId");
            builder.Property(x => x.Name).HasColumnType("nvarchar(500)");
        }
    }
}


