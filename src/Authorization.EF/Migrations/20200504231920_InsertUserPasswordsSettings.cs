using Microsoft.EntityFrameworkCore.Migrations;

namespace Authorization.EF.Migrations
{
    public partial class InsertUserPasswordsSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SettingsEntries",
                columns: new[] { "SettingsEntryId", "Value" },
                values: new object[,]
                {
                    { "Jwt:PasswordExpiredValidityPeriod", "00:05:00" },
                    { "UserPasswords:KeepUserPasswordsCount", "5" },
                    { "UserPasswords:MaxAccessFailedCount", "6" },
                    { "UserPasswords:LockoutPeriod", "00:30:00" },
                    { "UserPasswords:PasswordExpirationPeriod", "90.00:00:00" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Jwt:PasswordExpiredValidityPeriod");

            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "UserPasswords:KeepUserPasswordsCount");

            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "UserPasswords:LockoutPeriod");

            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "UserPasswords:MaxAccessFailedCount");

            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "UserPasswords:PasswordExpirationPeriod");
        }
    }
}


