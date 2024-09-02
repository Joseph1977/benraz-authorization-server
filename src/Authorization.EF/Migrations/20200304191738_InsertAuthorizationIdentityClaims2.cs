using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Authorization.EF.Migrations
{
    public partial class InsertAuthorizationIdentityClaims2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Claims",
                columns: new[] { "ClaimId", "CreateTimeUtc", "Type", "UpdateTimeUtc", "Value" },
                values: new object[,]
                {
                    { new Guid("e837054c-4ad2-4792-ac9a-e45f91e5ddb3"), new DateTime(2020, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-user-email-read" },
                    { new Guid("f7958c48-3473-493e-902b-e99a2b930518"), new DateTime(2020, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-user-phone-read" }
                });

            migrationBuilder.InsertData(
                table: "RoleClaims",
                columns: new[] { "RoleClaimId", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { -25, "claim", "authorization-user-email-read", "89117c41-23b9-4330-ada1-57464fc84aa0" },
                    { -26, "claim", "authorization-user-phone-read", "89117c41-23b9-4330-ada1-57464fc84aa0" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("e837054c-4ad2-4792-ac9a-e45f91e5ddb3"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("f7958c48-3473-493e-902b-e99a2b930518"));

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -26);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -25);
        }
    }
}


