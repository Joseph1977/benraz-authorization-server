using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Authorization.EF.Migrations
{
    public partial class RemoveUserClaimMappingEntries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserClaimMappingEntries");

            migrationBuilder.DeleteData(
                table: "ApplicationUrls",
                keyColumn: "ApplicationUrlTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ApplicationUrlTypes",
                keyColumn: "ApplicationUrlTypeId",
                keyValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserClaimMappingEntries",
                columns: table => new
                {
                    UserClaimMappingEntryId = table.Column<Guid>(nullable: false),
                    CreateTimeUtc = table.Column<DateTime>(nullable: false),
                    SourceType = table.Column<string>(nullable: true),
                    SourceValue = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    UpdateTimeUtc = table.Column<DateTime>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaimMappingEntries", x => x.UserClaimMappingEntryId);
                });

            migrationBuilder.InsertData(
                table: "ApplicationUrlTypes",
                columns: new[] { "ApplicationUrlTypeId", "Name" },
                values: new object[] { 1, "Claims URL" });

            migrationBuilder.InsertData(
                table: "UserClaimMappingEntries",
                columns: new[] { "UserClaimMappingEntryId", "CreateTimeUtc", "SourceType", "SourceValue", "Type", "UpdateTimeUtc", "Value" },
                values: new object[,]
                {
                    { new Guid("15f3d112-5154-485b-b7d3-c83e50da08bb"), new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "groups", "AuthorizationServer-Read", "roles", new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Read" },
                    { new Guid("dd19cb61-0bd8-4af0-a574-724ae3c9e28f"), new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "groups", "AuthorizationServer-Write", "roles", new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Write" },
                    { new Guid("65f9f76f-1122-4b9b-9764-740feaa5b269"), new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "groups", "AuthorizationServer-Admin", "roles", new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Administrator" }
                });
        }
    }
}


