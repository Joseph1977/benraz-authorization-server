using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authorization.EF.Migrations
{
    public partial class AddEmailsProviderSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:ConfirmEmailTemplateId");

            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:Password");

            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:ResetPasswordTemplateId");

            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:Username");

            migrationBuilder.UpdateData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:ResetPasswordSubject",
                column: "Description",
                value: "Reset password email subject");

            migrationBuilder.InsertData(
                table: "SettingsEntries",
                columns: new[] { "SettingsEntryId", "Description", "Value" },
                values: new object[,]
                {
                    { "Emails:ServiceProvider:ServiceType", "Email service provider type; 1 - Benraz services", "1" },
                    { "Emails:ServiceProvider:Benraz:BaseUrl", "Benraz email services base URL", null },
                    { "Emails:ServiceProvider:Benraz:TemplateId", "Benraz email services default template identifier", "c7a628da-d901-4916-9618-984eeaa28fda" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:ServiceProvider:ServiceType");

            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:ServiceProvider:Benraz:BaseUrl");

            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:ServiceProvider:Benraz:TemplateId");

            migrationBuilder.UpdateData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:ResetPasswordSubject",
                column: "Description",
                value: null);

            migrationBuilder.InsertData(
                table: "SettingsEntries",
                columns: new[] { "SettingsEntryId", "Description", "Value" },
                values: new object[,]
                {
                    { "Emails:ConfirmEmailTemplateId", "Confirmation email template identifier", "8cd5b7d0-df9e-4b66-93ff-1f7e0983e1e4" },
                    { "Emails:Password", null, null },
                    { "Emails:ResetPasswordTemplateId", null, "366ea284-d131-47d4-9b27-4c28f71dea8a" },
                    { "Emails:Username", null, null }
                });
        }
    }
}
