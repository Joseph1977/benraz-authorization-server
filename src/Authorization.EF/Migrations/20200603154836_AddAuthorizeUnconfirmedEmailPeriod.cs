using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Authorization.EF.Migrations
{
    public partial class AddAuthorizeUnconfirmedEmailPeriod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTimeUtc",
                table: "Users",
                nullable: true);

            migrationBuilder.InsertData(
                table: "SettingsEntries",
                columns: new[] { "SettingsEntryId", "Value" },
                values: new object[] { "Authorization:AuthorizeUnconfirmedEmailPeriod", "00:00:00" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "Authorization:AuthorizeUnconfirmedEmailPeriod");

            migrationBuilder.DropColumn(
                name: "CreateTimeUtc",
                table: "Users");
        }
    }
}


