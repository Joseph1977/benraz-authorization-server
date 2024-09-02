using Authorization.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.EF.Configurations
{
    class UserMfaConfiguration : IEntityTypeConfiguration<UserMfa>
    {
        public void Configure(EntityTypeBuilder<UserMfa> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("UserMfaId");
        }
    }
}
