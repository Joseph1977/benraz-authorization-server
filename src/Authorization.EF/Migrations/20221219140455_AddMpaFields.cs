using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authorization.EF.Migrations
{
    public partial class AddMpaFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCodeConsumed",
                table: "UserMfa",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSendSuccess",
                table: "UserMfa",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SendError",
                table: "UserMfa",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SendThrough",
                table: "UserMfa",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "SettingsEntries",
                columns: new[] { "SettingsEntryId", "Description", "Value" },
                values: new object[,]
                {
                    { "Authorization:AccessTokenMfaCodeLength", "Access token code length", "10" },
                    { "Authorization:MfaCodeLength", "Mfa code length", "6" },
                    { "Emails:MfaCodeSubject", "Account verification code email subject", "Account verification" },
                    { "Jwt:ConfirmEmailValidityPeriod", null, "1.00:00:00" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Authorization:AccessTokenMfaCodeLength");

            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Authorization:MfaCodeLength");

            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Emails:MfaCodeSubject");

            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Jwt:ConfirmEmailValidityPeriod");

            migrationBuilder.DropColumn(
                name: "IsCodeConsumed",
                table: "UserMfa");

            migrationBuilder.DropColumn(
                name: "IsSendSuccess",
                table: "UserMfa");

            migrationBuilder.DropColumn(
                name: "SendError",
                table: "UserMfa");

            migrationBuilder.DropColumn(
                name: "SendThrough",
                table: "UserMfa");
        }
    }
}
