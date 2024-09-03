using Authorization.Domain.SsoProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.EF.Configurations
{
    class SsoProviderConfiguration : IEntityTypeConfiguration<SsoProvider>
    {
        public void Configure(EntityTypeBuilder<SsoProvider> builder)
        {
            builder.ToTable("SsoProviders");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("SsoProviderId");
            builder.Property(x => x.Name).HasColumnType("nvarchar(500)");
        }
    }
}


