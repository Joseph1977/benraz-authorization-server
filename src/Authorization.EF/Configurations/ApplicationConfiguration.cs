using Authorization.Domain.Applications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.EF.Configurations
{
    class ApplicationConfiguration : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.ToTable("Applications");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("ApplicationId");
            builder.Property(x => x.Name).HasColumnType("nvarchar(500)");
            builder.Property(x => x.Audience).HasColumnType("nvarchar(500)");
            builder.Property(x => x.AccessTokenCookieName).HasColumnType("nvarchar(500)");
            builder.Property(x => x.AccessTokenValidityPeriod).HasColumnType("nvarchar(100)");
            builder.Property(x => x.CreatedBy).HasColumnType("nvarchar(100)");
            builder.Property(x => x.UpdatedBy).HasColumnType("nvarchar(100)");

            builder
                .HasMany(x => x.SsoConnections)
                .WithOne()
                .HasForeignKey(x => x.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(x => x.Urls)
                .WithOne()
                .HasForeignKey(x => x.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}


