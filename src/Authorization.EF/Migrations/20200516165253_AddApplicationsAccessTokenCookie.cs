using Microsoft.EntityFrameworkCore.Migrations;

namespace Authorization.EF.Migrations
{
    public partial class AddApplicationsAccessTokenCookie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Audience",
                table: "Applications",
                type: "nvarchar(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccessTokenCookieName",
                table: "Applications",
                type: "nvarchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAccessTokenCookieEnabled",
                table: "Applications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAccessTokenFragmentDisabled",
                table: "Applications",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessTokenCookieName",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "IsAccessTokenCookieEnabled",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "IsAccessTokenFragmentDisabled",
                table: "Applications");

            migrationBuilder.AlterColumn<string>(
                name: "Audience",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldNullable: true);
        }
    }
}


