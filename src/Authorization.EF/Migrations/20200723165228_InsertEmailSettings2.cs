using Microsoft.EntityFrameworkCore.Migrations;

namespace Authorization.EF.Migrations
{
    public partial class InsertEmailSettings2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:ResetPasswordTemplateId",
                column: "Value",
                value: "366ea284-d131-47d4-9b27-4c28f71dea8a");

            migrationBuilder.InsertData(
                table: "SettingsEntries",
                columns: new[] { "SettingsEntryId", "Value" },
                values: new object[] { "Emails:CompanyPhone", "1-888-497-5499" });

            migrationBuilder.InsertData(
                table: "SettingsEntries",
                columns: new[] { "SettingsEntryId", "Value" },
                values: new object[] { "Emails:CompanyEmail", "support@benraz.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:CompanyEmail");

            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:CompanyPhone");

            migrationBuilder.UpdateData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:ResetPasswordTemplateId",
                column: "Value",
                value: "118af26f-b313-4a5e-a1a8-11a64e85b1b5");
        }
    }
}


