using Microsoft.EntityFrameworkCore.Migrations;

namespace Authorization.EF.Migrations
{
    public partial class UpdatePasswordExpirationPeriodSettingsEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "UserPasswords:PasswordExpirationPeriod",
                column: "Value",
                value: null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "UserPasswords:PasswordExpirationPeriod",
                column: "Value",
                value: "90.00:00:00");
        }
    }
}


