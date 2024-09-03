using Microsoft.EntityFrameworkCore.Migrations;

namespace Authorization.EF.Migrations
{
    public partial class UpdateSettingsEntries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SettingsEntries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "SettingsEntries");
        }
    }
}


