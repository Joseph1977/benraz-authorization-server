using Authorization.Domain.SsoConnections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.EF.Configurations
{
    class SsoConnectionConfiguration : IEntityTypeConfiguration<SsoConnection>
    {
        public void Configure(EntityTypeBuilder<SsoConnection> builder)
        {
            builder.ToTable("SsoConnections");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("SsoConnectionId");
            builder.Property(x => x.SsoProviderCode).HasColumnName("SsoProviderId");
            builder.Property(x => x.AuthorizationUrl).HasColumnType("nvarchar(2000)");
            builder.Property(x => x.TokenUrl).HasColumnType("nvarchar(2000)");
            builder.Property(x => x.ClientId).HasColumnType("nvarchar(500)");
            builder.Property(x => x.ClientSecret).HasColumnType("nvarchar(500)");
            builder.Property(x => x.Scope).HasColumnType("nvarchar(2000)");

            builder
                .HasOne(x => x.SsoProvider)
                .WithMany()
                .HasForeignKey(x => x.SsoProviderCode)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}


