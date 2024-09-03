using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authorization.EF.Migrations
{
    public partial class ChangeMfaCodeSubjectText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:MfaCodeSubject",
                columns: new[] { "Description", "Value" },
                values: new object[] { "Mfa code email subject and replacing the {0} by action type", "Your BENRAZ {0} code" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:MfaCodeSubject",
                columns: new[] { "Description", "Value" },
                values: new object[] { "Account verification code email subject", "Account verification" });
        }
    }
}
