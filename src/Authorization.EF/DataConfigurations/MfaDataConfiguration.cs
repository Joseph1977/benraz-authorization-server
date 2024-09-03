using Authorization.Domain.Mfa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Benraz.Infrastructure.Domain.Common;

namespace Authorization.EF.DataConfigurations
{
    class MfaDataConfiguration : IEntityTypeConfiguration<Mfa>
    {
        public void Configure(EntityTypeBuilder<Mfa> builder)
        {
            builder.HasData(
                Create(MfaCode.ConfirmEmail, "ConfirmEmail"),
                Create(MfaCode.ResetPassword, "ResetPassword"),
                Create(MfaCode.Action, "Action"));
        }

        private Mfa Create(MfaCode id, string name)
        {
            return new Mfa
            {
                Id = id,
                Name = name
            };
        }
    }
}
