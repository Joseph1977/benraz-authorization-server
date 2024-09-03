using Authorization.Domain.ApplicationTokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.EF.Configurations
{
    class ApplicationTokenCustomFieldConfiguration : IEntityTypeConfiguration<ApplicationTokenCustomField>
    {
        public void Configure(EntityTypeBuilder<ApplicationTokenCustomField> builder)
        {
            builder.ToTable("ApplicationTokenCustomFields");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("ApplicationTokenCustomFieldId");
            builder.Property(x => x.Key).HasColumnType("nvarchar(500)");
        }
    }
}


