using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authorization.EF.Migrations
{
    public partial class AddedDefaultRolesListSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SettingsEntries",
                columns: new[] { "SettingsEntryId", "Description", "Value" },
                values: new object[] { "User:DefaultRolesList", "Default roles list.", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "User:DefaultRolesList");
        }
    }
}
