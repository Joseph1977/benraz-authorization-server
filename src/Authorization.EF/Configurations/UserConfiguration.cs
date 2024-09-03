using Authorization.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.EF.Configurations;

class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.Property(x => x.Id).HasColumnName("UserId");
        builder.Property(x => x.FullName).HasColumnType("nvarchar(500)");
        builder.Property(x => x.PhoneNumber).HasColumnType("nvarchar(500)");

        builder.HasIndex(x => x.NormalizedEmail).IsUnique(true);
        builder.HasIndex(x => x.PhoneNumber).IsUnique(false);
        builder.HasIndex(x => x.Email);
        builder.HasIndex(x => x.FullName);

        builder
            .HasOne<UserStatus>()
            .WithMany()
            .HasForeignKey(x => x.StatusCode)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.UserRoles)
            .WithOne()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Address)
            .WithMany()
            .HasForeignKey(x => x.AddressId);

        builder
            .HasMany(x => x.UserMfas)
            .WithOne()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}