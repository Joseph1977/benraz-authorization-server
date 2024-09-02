using Microsoft.EntityFrameworkCore.Migrations;

namespace Authorization.EF.Migrations
{
    public partial class InsertGoogleSsoProvider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SsoProviders",
                columns: new[] { "SsoProviderId", "Name" },
                values: new object[] { 4, "Google" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SsoProviders",
                keyColumn: "SsoProviderId",
                keyValue: 4);
        }
    }
}


