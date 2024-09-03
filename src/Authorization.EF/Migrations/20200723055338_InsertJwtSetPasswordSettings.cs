using Microsoft.EntityFrameworkCore.Migrations;

namespace Authorization.EF.Migrations
{
    public partial class InsertJwtSetPasswordSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SettingsEntries",
                columns: new[] { "SettingsEntryId", "Value" },
                values: new object[] { "Jwt:SetPasswordValidityPeriod", "1.00:00:00" });

            migrationBuilder.InsertData(
                table: "SettingsEntries",
                columns: new[] { "SettingsEntryId", "Value" },
                values: new object[] { "Jwt:SetPasswordAudience", "Authorization" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Jwt:SetPasswordAudience");

            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Jwt:SetPasswordValidityPeriod");
        }
    }
}


