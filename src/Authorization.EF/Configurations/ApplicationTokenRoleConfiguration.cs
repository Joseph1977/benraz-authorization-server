using Authorization.Domain.ApplicationTokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.EF.Configurations
{
    class ApplicationTokenRoleConfiguration : IEntityTypeConfiguration<ApplicationTokenRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationTokenRole> builder)
        {
            builder.ToTable("ApplicationTokenRoles");

            builder.HasKey(x => new { x.ApplicationTokenId, x.RoleId });

            builder
                .HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId);
        }
    }
}


