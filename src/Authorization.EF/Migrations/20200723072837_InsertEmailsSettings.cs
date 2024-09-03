using Microsoft.EntityFrameworkCore.Migrations;

namespace Authorization.EF.Migrations
{
    public partial class InsertEmailsSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SettingsEntries",
                columns: new[] { "SettingsEntryId", "Value" },
                values: new object[,]
                {
                    { "Emails:Username", null },
                    { "Emails:Password", null },
                    { "Emails:From", "info@benraz.com" },
                    { "Emails:FromDisplayName", "Benraz" },
                    { "Emails:CompanyName", "Benraz" },
                    { "Emails:CompanyLogoUrl", "https://www.benraz.com/wp-content/themes/benraz-by-dorzki/assets/images/logo.svg" },
                    { "Emails:ResetPasswordSubject", "Reset password" },
                    { "Emails:ResetPasswordTemplateId", "118af26f-b313-4a5e-a1a8-11a64e85b1b5" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:CompanyLogoUrl");

            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:CompanyName");

            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:From");

            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:FromDisplayName");

            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:Password");

            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:ResetPasswordSubject");

            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:ResetPasswordTemplateId");

            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:Username");
        }
    }
}


