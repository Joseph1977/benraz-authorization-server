using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authorization.EF.Migrations
{
    public partial class AddMfaCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "VerificationCodeExpirationDate",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Mfa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mfa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserMfa",
                columns: table => new
                {
                    UserMfaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMfa", x => x.UserMfaId);
                    table.ForeignKey(
                        name: "FK_UserMfa_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Mfa",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "ConfirmEmail" },
                    { 2, "ResetPassword" },
                    { 3, "Action" }
                });

            migrationBuilder.InsertData(
                table: "SettingsEntries",
                columns: new[] { "SettingsEntryId", "Description", "Value" },
                values: new object[] { "User:VerificationCodeCooldown", "Verification code cooldown.", "00:01:00" });

            migrationBuilder.CreateIndex(
                name: "IX_UserMfa_UserId",
                table: "UserMfa",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mfa");

            migrationBuilder.DropTable(
                name: "UserMfa");

            migrationBuilder.DeleteData(
                table: "SettingsEntries",
                keyColumn: "SettingsEntryId",
                keyValue: "User:VerificationCodeCooldown");

            migrationBuilder.AddColumn<string>(
                name: "VerificationCode",
                table: "Users",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VerificationCodeExpirationDate",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }
    }
}
