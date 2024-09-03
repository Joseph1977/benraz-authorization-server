using Authorization.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.EF.Configurations
{
    class SettingsEntryConfiguration : IEntityTypeConfiguration<SettingsEntry>
    {
        public void Configure(EntityTypeBuilder<SettingsEntry> builder)
        {
            builder.ToTable("SettingsEntries");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("SettingsEntryId");
        }
    }
}


