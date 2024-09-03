using Authorization.Domain.Applications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.EF.DataConfigurations
{
    class ApplicationUrlTypeDataConfiguration : IEntityTypeConfiguration<ApplicationUrlType>
    {
        public void Configure(EntityTypeBuilder<ApplicationUrlType> builder)
        {
            builder.HasData(
                Create(ApplicationUrlTypeCode.Callback, "Authorization callback URL"));
        }

        private ApplicationUrlType Create(ApplicationUrlTypeCode typeCode, string name)
        {
            return new ApplicationUrlType
            {
                Id = typeCode,
                Name = name
            };
        }
    }
}


