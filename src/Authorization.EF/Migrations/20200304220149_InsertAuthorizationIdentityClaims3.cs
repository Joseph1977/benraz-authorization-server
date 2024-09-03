using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Authorization.EF.Migrations
{
    public partial class InsertAuthorizationIdentityClaims3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Claims",
                columns: new[] { "ClaimId", "CreateTimeUtc", "Type", "UpdateTimeUtc", "Value" },
                values: new object[] { new Guid("28aa11a0-6544-4bb2-96bd-6a2a98aebcdd"), new DateTime(2020, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-user-add" });

            migrationBuilder.InsertData(
                table: "RoleClaims",
                columns: new[] { "RoleClaimId", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[] { -27, "claim", "authorization-user-add", "89117c41-23b9-4330-ada1-57464fc84aa0" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("28aa11a0-6544-4bb2-96bd-6a2a98aebcdd"));

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -27);
        }
    }
}


