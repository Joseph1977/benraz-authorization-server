using Authorization.Domain.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.EF.Configurations
{
    class IdentityClaimConfiguration : IEntityTypeConfiguration<IdentityClaim>
    {
        public void Configure(EntityTypeBuilder<IdentityClaim> builder)
        {
            builder.ToTable("Claims");

            builder.Property(x => x.Id).HasColumnName("ClaimId");
        }
    }
}


