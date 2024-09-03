using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authorization.EF.Migrations
{
    public partial class AddBenrazServiceProviderSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SettingsEntries",
                columns: new[] { "SettingsEntryId", "Description", "Value" },
                values: new object[] { "Emails:ServiceProvider:Benraz:AccessToken", "Benraz email services access token", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:ServiceProvider:Benraz:AccessToken");
        }
    }
}
