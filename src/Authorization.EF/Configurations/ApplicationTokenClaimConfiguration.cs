using Authorization.Domain.ApplicationTokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.EF.Configurations
{
    class ApplicationTokenClaimConfiguration : IEntityTypeConfiguration<ApplicationTokenClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationTokenClaim> builder)
        {
            builder.ToTable("ApplicationTokenClaims");

            builder.HasKey(x => new { x.ApplicationTokenId, x.ClaimId });

            builder
                .HasOne(x => x.Claim)
                .WithMany()
                .HasForeignKey(x => x.ClaimId);
        }
    }
}


