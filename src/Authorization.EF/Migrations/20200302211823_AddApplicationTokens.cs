using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Authorization.EF.Migrations
{
    public partial class AddApplicationTokens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationTokens",
                columns: table => new
                {
                    ApplicationTokenId = table.Column<Guid>(nullable: false),
                    CreateTimeUtc = table.Column<DateTime>(nullable: false),
                    UpdateTimeUtc = table.Column<DateTime>(nullable: false),
                    ApplicationId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    Value = table.Column<string>(nullable: true),
                    ExpirationTimeUtc = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationTokens", x => x.ApplicationTokenId);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationTokenClaims",
                columns: table => new
                {
                    ApplicationTokenId = table.Column<Guid>(nullable: false),
                    ClaimId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationTokenClaims", x => new { x.ApplicationTokenId, x.ClaimId });
                    table.ForeignKey(
                        name: "FK_ApplicationTokenClaims_ApplicationTokens_ApplicationTokenId",
                        column: x => x.ApplicationTokenId,
                        principalTable: "ApplicationTokens",
                        principalColumn: "ApplicationTokenId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationTokenClaims_Claims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claims",
                        principalColumn: "ClaimId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationTokenRoles",
                columns: table => new
                {
                    ApplicationTokenId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationTokenRoles", x => new { x.ApplicationTokenId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_ApplicationTokenRoles_ApplicationTokens_ApplicationTokenId",
                        column: x => x.ApplicationTokenId,
                        principalTable: "ApplicationTokens",
                        principalColumn: "ApplicationTokenId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationTokenRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTokenClaims_ClaimId",
                table: "ApplicationTokenClaims",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTokenRoles_RoleId",
                table: "ApplicationTokenRoles",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationTokenClaims");

            migrationBuilder.DropTable(
                name: "ApplicationTokenRoles");

            migrationBuilder.DropTable(
                name: "ApplicationTokens");
        }
    }
}


