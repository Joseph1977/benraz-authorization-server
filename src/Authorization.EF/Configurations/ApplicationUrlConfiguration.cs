using Authorization.Domain.Applications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.EF.Configurations
{
    class ApplicationUrlConfiguration : IEntityTypeConfiguration<ApplicationUrl>
    {
        public void Configure(EntityTypeBuilder<ApplicationUrl> builder)
        {
            builder.ToTable("ApplicationUrls");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("ApplicationUrlId");
            builder.Property(x => x.TypeCode).HasColumnName("ApplicationUrlTypeId");
            builder.Property(x => x.Url).HasColumnType("nvarchar(2000)");

            builder.HasIndex(x => x.TypeCode).IsUnique(false);

            builder
                .HasOne<ApplicationUrlType>()
                .WithOne()
                .HasForeignKey<ApplicationUrl>(x => x.TypeCode)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}


