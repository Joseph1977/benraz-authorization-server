using Authorization.Domain.Applications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.EF.Configurations
{
    class ApplicationUrlTypeConfiguration : IEntityTypeConfiguration<ApplicationUrlType>
    {
        public void Configure(EntityTypeBuilder<ApplicationUrlType> builder)
        {
            builder.ToTable("ApplicationUrlTypes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("ApplicationUrlTypeId");
            builder.Property(x => x.Name).HasColumnType("nvarchar(500)");
        }
    }
}


