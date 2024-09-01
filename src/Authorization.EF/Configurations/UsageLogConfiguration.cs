using Authorization.Domain.UsageLogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.EF.Configurations
{
    class UsageLogConfiguration : IEntityTypeConfiguration<UsageLog>
    {
        public void Configure(EntityTypeBuilder<UsageLog> builder)
        {
            builder.ToTable("UsageLogs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("UsageLogId");
            builder.Property(x => x.UserName).HasMaxLength(500);
            builder.Property(x => x.IPAddress).HasMaxLength(100);
            builder.Property(x => x.Action).HasMaxLength(2000);
        }
    }
}


