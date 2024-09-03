using Microsoft.EntityFrameworkCore.Migrations;

namespace Authorization.EF.Migrations
{
    public partial class InsertIdentityRoles2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "23b143c8-3c3a-42dd-949c-dd1890ce778b", "524fc668-3c44-4cc1-9bf6-790fb537b15b", "New applicant", "NEW APPLICANT" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "23b143c8-3c3a-42dd-949c-dd1890ce778b");
        }
    }
}


