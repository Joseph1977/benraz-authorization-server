using Authorization.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.EF.Configurations
{
    class UserPasswordConfiguration : IEntityTypeConfiguration<UserPassword>
    {
        public void Configure(EntityTypeBuilder<UserPassword> builder)
        {
            builder.ToTable("UserPasswords");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("UserPasswordId");

            builder
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}


