using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.EF.DataConfigurations
{
    class IdentityRoleClaimDataConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
        {
            builder.HasData(
                Create(-1, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-application-read"),
                Create(-2, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-application-add"),
                Create(-3, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-application-update"),
                Create(-4, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-application-delete"),
                Create(-5, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-user-read"),
                Create(-6, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-user-update"),
                Create(-7, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-user-delete"),
                Create(-8, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-user-status-read"),
                Create(-9, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-user-status-suspend"),
                Create(-10, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-user-status-block"),
                Create(-11, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-user-role-read"),
                Create(-12, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-user-role-update"),
                Create(-13, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-user-claim-read"),
                Create(-14, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-user-claim-update"),
                Create(-15, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-user-email-verify"),
                Create(-16, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-user-phone-verify"),
                Create(-17, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-user-password-reset"),
                Create(-18, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-role-read"),
                Create(-19, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-role-add"),
                Create(-20, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-role-update"),
                Create(-21, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-role-delete"),
                Create(-22, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-claim-read"),
                Create(-23, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-claim-add"),
                Create(-24, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-claim-delete"),
                Create(-25, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-user-email-read"),
                Create(-26, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-user-phone-read"),
                Create(-27, "89117c41-23b9-4330-ada1-57464fc84aa0", "claim", "authorization-user-add"));
        }

        private IdentityRoleClaim<string> Create(int id, string roleId, string claimType, string claimValue)
        {
            return new IdentityRoleClaim<string>
            {
                Id = id,
                RoleId = roleId,
                ClaimType = claimType,
                ClaimValue = claimValue
            };
        }
    }
}


