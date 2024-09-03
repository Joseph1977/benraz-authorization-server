using Authorization.Domain.SsoProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Benraz.Infrastructure.Domain.Authorization;

namespace Authorization.EF.DataConfigurations
{
    class SsoProviderDataConfiguration : IEntityTypeConfiguration<SsoProvider>
    {
        public void Configure(EntityTypeBuilder<SsoProvider> builder)
        {
            builder.HasData(
                Create(SsoProviderCode.Internal, "Internal"),
                Create(SsoProviderCode.Microsoft, "Microsoft"),
                Create(SsoProviderCode.Facebook, "Facebook"),
                Create(SsoProviderCode.Google, "Google"));
        }

        private SsoProvider Create(SsoProviderCode id, string name)
        {
            return new SsoProvider
            {
                Id = id,
                Name = name
            };
        }
    }
}


