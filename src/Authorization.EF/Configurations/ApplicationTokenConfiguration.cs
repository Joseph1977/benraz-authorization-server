using Authorization.Domain.ApplicationTokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.EF.Configurations
{
    class ApplicationTokenConfiguration : IEntityTypeConfiguration<ApplicationToken>
    {
        public void Configure(EntityTypeBuilder<ApplicationToken> builder)
        {
            builder.ToTable("ApplicationTokens");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("ApplicationTokenId");
            builder.Property(x => x.Name).HasColumnType("nvarchar(500)");
            builder.Property(x => x.CreatedBy).HasColumnType("nvarchar(100)");

            builder
                .HasMany(x => x.Roles)
                .WithOne()
                .HasForeignKey(x => x.ApplicationTokenId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(x => x.Claims)
                .WithOne()
                .HasForeignKey(x => x.ApplicationTokenId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}


