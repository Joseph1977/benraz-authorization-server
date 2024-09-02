using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Authorization.EF.Migrations
{
    public partial class AddApplicationTokenCustomFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationTokenCustomFields",
                columns: table => new
                {
                    ApplicationTokenCustomFieldId = table.Column<Guid>(nullable: false),
                    ApplicationTokenId = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationTokenCustomFields", x => x.ApplicationTokenCustomFieldId);
                    table.ForeignKey(
                        name: "FK_ApplicationTokenCustomFields_ApplicationTokens_ApplicationTokenId",
                        column: x => x.ApplicationTokenId,
                        principalTable: "ApplicationTokens",
                        principalColumn: "ApplicationTokenId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTokenCustomFields_ApplicationTokenId",
                table: "ApplicationTokenCustomFields",
                column: "ApplicationTokenId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationTokenCustomFields");
        }
    }
}


