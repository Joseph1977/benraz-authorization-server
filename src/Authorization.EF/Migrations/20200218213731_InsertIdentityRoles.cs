using Microsoft.EntityFrameworkCore.Migrations;

namespace Authorization.EF.Migrations
{
    public partial class InsertIdentityRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d7b4f6d4-9cab-49ce-abf2-c0bc6d5d1ae2", "96fd1b42-0720-4de0-be3e-d51b5bb3476d", "Employee", "EMPLOYEE" },
                    { "d14a3723-2ca1-49b2-9d58-50220836053d", "b1cc36c5-7958-450d-9363-70d81b4f02ab", "Organization user", "ORGANIZATION USER" },
                    { "25c5290f-91bc-4c47-ad4f-9fc01f6191cd", "3cc4aba1-6b94-4af8-959d-e5cb3be30ace", "Tenant", "TENANT" },
                    { "f80e34a1-e155-4a87-b2d5-7d1367963343", "bfb01b6e-d093-4e48-af66-08bcab57f9c2", "Property manager", "PROPERTY MANAGER" },
                    { "48aa65a2-dc1c-4ce9-bf07-ee4c05973e58", "bbebc5db-8168-4ad1-8581-7d5e5d03d90f", "Service provider", "SERVICE PROVIDER" },
                    { "2ada0e62-3793-4d90-b635-80e334362e65", "2e0b2011-c2f6-4837-8aa8-ff5c1aa79201", "Owner", "OWNER" },
                    { "c5b42e20-e5b0-4f5c-a968-dd0050e21070", "2162f58e-8faf-4d89-9d1d-3feda66a0ee4", "Partner", "PARTNER" },
                    { "042858f3-df80-41a9-b2f3-eff9a4a50ba9", "6e07524f-99da-47e4-9ea0-e510de7d0d55", "Internal server", "INTERNAL SERVER" },
                    { "e3ffc4ce-4dc1-4ea4-91c4-7939fd9a2620", "54267f41-15fe-447c-896a-1cdd702d7ac2", "External server", "EXTERNAL SERVER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "042858f3-df80-41a9-b2f3-eff9a4a50ba9");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "25c5290f-91bc-4c47-ad4f-9fc01f6191cd");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "2ada0e62-3793-4d90-b635-80e334362e65");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "48aa65a2-dc1c-4ce9-bf07-ee4c05973e58");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "c5b42e20-e5b0-4f5c-a968-dd0050e21070");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "d14a3723-2ca1-49b2-9d58-50220836053d");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "d7b4f6d4-9cab-49ce-abf2-c0bc6d5d1ae2");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "e3ffc4ce-4dc1-4ea4-91c4-7939fd9a2620");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "f80e34a1-e155-4a87-b2d5-7d1367963343");
        }
    }
}


