using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.EF.DataConfigurations
{
    class IdentityRoleDataConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                Create("23b143c8-3c3a-42dd-949c-dd1890ce778b", "New applicant", "524fc668-3c44-4cc1-9bf6-790fb537b15b"),
                Create("d7b4f6d4-9cab-49ce-abf2-c0bc6d5d1ae2", "Employee", "96fd1b42-0720-4de0-be3e-d51b5bb3476d"),
                Create("d14a3723-2ca1-49b2-9d58-50220836053d", "Organization user", "b1cc36c5-7958-450d-9363-70d81b4f02ab"),
                Create("25c5290f-91bc-4c47-ad4f-9fc01f6191cd", "Tenant", "3cc4aba1-6b94-4af8-959d-e5cb3be30ace"),
                Create("f80e34a1-e155-4a87-b2d5-7d1367963343", "Property manager", "bfb01b6e-d093-4e48-af66-08bcab57f9c2"),
                Create("48aa65a2-dc1c-4ce9-bf07-ee4c05973e58", "Service provider", "bbebc5db-8168-4ad1-8581-7d5e5d03d90f"),
                Create("2ada0e62-3793-4d90-b635-80e334362e65", "Property owner", "2e0b2011-c2f6-4837-8aa8-ff5c1aa79201"),
                Create("c5b42e20-e5b0-4f5c-a968-dd0050e21070", "Partner", "2162f58e-8faf-4d89-9d1d-3feda66a0ee4"),
                Create("042858f3-df80-41a9-b2f3-eff9a4a50ba9", "Internal server", "6e07524f-99da-47e4-9ea0-e510de7d0d55"),
                Create("e3ffc4ce-4dc1-4ea4-91c4-7939fd9a2620", "External server", "54267f41-15fe-447c-896a-1cdd702d7ac2"),
                Create("89117c41-23b9-4330-ada1-57464fc84aa0", "authorization-administrator", "1b944e7f-b96f-48b1-a71a-4d54f72ade21"));
        }

        private IdentityRole Create(string id, string name, string concurrencyStamp)
        {
            return new IdentityRole
            {
                Id = id,
                Name = name,
                NormalizedName = name.ToUpperInvariant(),
                ConcurrencyStamp = concurrencyStamp,
            };
        }
    }
}


