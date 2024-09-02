using Microsoft.EntityFrameworkCore.Migrations;

namespace Authorization.EF.Migrations
{
    public partial class ChangeApplicationUrlsTypeIdIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApplicationUrls_ApplicationUrlTypeId",
                table: "ApplicationUrls");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUrls_ApplicationUrlTypeId",
                table: "ApplicationUrls",
                column: "ApplicationUrlTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApplicationUrls_ApplicationUrlTypeId",
                table: "ApplicationUrls");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUrls_ApplicationUrlTypeId",
                table: "ApplicationUrls",
                column: "ApplicationUrlTypeId",
                unique: true);
        }
    }
}


