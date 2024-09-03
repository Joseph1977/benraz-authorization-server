using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Authorization.EF.Migrations
{
    public partial class InsertAuthorizationIdentityClaims : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Claims",
                columns: new[] { "ClaimId", "CreateTimeUtc", "Type", "UpdateTimeUtc", "Value" },
                values: new object[,]
                {
                    { new Guid("418270bb-d7ad-435b-8c8e-092d27da81b5"), new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-application-read" },
                    { new Guid("1a71b60b-7baf-47f6-8c08-950148b9fc43"), new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-claim-delete" },
                    { new Guid("b53984e1-2caa-487e-95fa-b95219c11e55"), new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-claim-add" },
                    { new Guid("21310d21-7b58-478f-96a0-8cf35dbb95c0"), new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-claim-read" },
                    { new Guid("2a1d29ea-60ce-4358-ac90-0e16a6fd2b96"), new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-role-delete" },
                    { new Guid("01af747a-c712-4c28-a95b-c7fd48434a47"), new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-role-update" },
                    { new Guid("aa0eccb7-b213-4838-8a2f-f48f2edd68b4"), new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-role-add" },
                    { new Guid("0f5e1f02-75df-4cc0-80b3-79072f8c1000"), new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-role-read" },
                    { new Guid("75baa2b9-b8d7-4d1c-a41c-e6cbc6a0d81f"), new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-user-password-reset" },
                    { new Guid("461469ab-6a88-4e01-8b9c-b50bc41f5c47"), new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-user-phone-verify" },
                    { new Guid("a55395b3-2c9f-42af-ab29-f5ce752b57c9"), new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-user-email-verify" },
                    { new Guid("6c9eb24a-39cd-4917-8483-08d7baf6949a"), new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-user-claim-update" },
                    { new Guid("e40f31ab-2155-4084-9710-7c6a889cf071"), new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-user-claim-read" },
                    { new Guid("ff115094-c2f8-46a2-9cb6-b1458f9ef92e"), new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-user-role-update" },
                    { new Guid("c132c8ba-c564-4667-8b56-a9fea1fc7d24"), new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-user-role-read" },
                    { new Guid("db1c7baa-1307-4b12-8136-60efa358036d"), new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-user-status-block" },
                    { new Guid("2cb96ac2-a8d0-44ac-8b8f-4e344e77345a"), new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-user-status-suspend" },
                    { new Guid("b69dd26d-c726-4cb1-aa94-d0e483daa8d2"), new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-user-status-read" },
                    { new Guid("f34e8539-03b4-47f5-98be-c9ae8808f527"), new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-user-delete" },
                    { new Guid("07ab8a18-fe49-489f-b772-0213d6abacc0"), new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-user-update" },
                    { new Guid("613f8d53-d4a9-4d7d-afc7-72190b8c6adb"), new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-user-read" },
                    { new Guid("b0f81152-cc31-49f9-bb4d-af49750fbebe"), new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-application-delete" },
                    { new Guid("49a2e542-d6da-4c7c-b2d7-13720b03619b"), new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-application-update" },
                    { new Guid("3e151682-a45b-4459-815b-36be325fdf59"), new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "authorization-application-add" }
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "2ada0e62-3793-4d90-b635-80e334362e65",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Property owner", "PROPERTY OWNER" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "89117c41-23b9-4330-ada1-57464fc84aa0", "1b944e7f-b96f-48b1-a71a-4d54f72ade21", "authorization-administrator", "AUTHORIZATION-ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "RoleClaims",
                columns: new[] { "RoleClaimId", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { -1, "claim", "authorization-application-read", "89117c41-23b9-4330-ada1-57464fc84aa0" },
                    { -22, "claim", "authorization-claim-read", "89117c41-23b9-4330-ada1-57464fc84aa0" },
                    { -21, "claim", "authorization-role-delete", "89117c41-23b9-4330-ada1-57464fc84aa0" },
                    { -20, "claim", "authorization-role-update", "89117c41-23b9-4330-ada1-57464fc84aa0" },
                    { -19, "claim", "authorization-role-add", "89117c41-23b9-4330-ada1-57464fc84aa0" },
                    { -18, "claim", "authorization-role-read", "89117c41-23b9-4330-ada1-57464fc84aa0" },
                    { -17, "claim", "authorization-user-password-reset", "89117c41-23b9-4330-ada1-57464fc84aa0" },
                    { -16, "claim", "authorization-user-phone-verify", "89117c41-23b9-4330-ada1-57464fc84aa0" },
                    { -15, "claim", "authorization-user-email-verify", "89117c41-23b9-4330-ada1-57464fc84aa0" },
                    { -14, "claim", "authorization-user-claim-update", "89117c41-23b9-4330-ada1-57464fc84aa0" },
                    { -13, "claim", "authorization-user-claim-read", "89117c41-23b9-4330-ada1-57464fc84aa0" },
                    { -12, "claim", "authorization-user-role-update", "89117c41-23b9-4330-ada1-57464fc84aa0" },
                    { -11, "claim", "authorization-user-role-read", "89117c41-23b9-4330-ada1-57464fc84aa0" },
                    { -10, "claim", "authorization-user-status-block", "89117c41-23b9-4330-ada1-57464fc84aa0" },
                    { -9, "claim", "authorization-user-status-suspend", "89117c41-23b9-4330-ada1-57464fc84aa0" },
                    { -8, "claim", "authorization-user-status-read", "89117c41-23b9-4330-ada1-57464fc84aa0" },
                    { -7, "claim", "authorization-user-delete", "89117c41-23b9-4330-ada1-57464fc84aa0" },
                    { -6, "claim", "authorization-user-update", "89117c41-23b9-4330-ada1-57464fc84aa0" },
                    { -5, "claim", "authorization-user-read", "89117c41-23b9-4330-ada1-57464fc84aa0" },
                    { -4, "claim", "authorization-application-delete", "89117c41-23b9-4330-ada1-57464fc84aa0" },
                    { -3, "claim", "authorization-application-update", "89117c41-23b9-4330-ada1-57464fc84aa0" },
                    { -2, "claim", "authorization-application-add", "89117c41-23b9-4330-ada1-57464fc84aa0" },
                    { -23, "claim", "authorization-claim-add", "89117c41-23b9-4330-ada1-57464fc84aa0" },
                    { -24, "claim", "authorization-claim-delete", "89117c41-23b9-4330-ada1-57464fc84aa0" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("01af747a-c712-4c28-a95b-c7fd48434a47"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("07ab8a18-fe49-489f-b772-0213d6abacc0"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("0f5e1f02-75df-4cc0-80b3-79072f8c1000"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("1a71b60b-7baf-47f6-8c08-950148b9fc43"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("21310d21-7b58-478f-96a0-8cf35dbb95c0"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("2a1d29ea-60ce-4358-ac90-0e16a6fd2b96"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("2cb96ac2-a8d0-44ac-8b8f-4e344e77345a"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("3e151682-a45b-4459-815b-36be325fdf59"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("418270bb-d7ad-435b-8c8e-092d27da81b5"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("461469ab-6a88-4e01-8b9c-b50bc41f5c47"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("49a2e542-d6da-4c7c-b2d7-13720b03619b"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("613f8d53-d4a9-4d7d-afc7-72190b8c6adb"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("6c9eb24a-39cd-4917-8483-08d7baf6949a"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("75baa2b9-b8d7-4d1c-a41c-e6cbc6a0d81f"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("a55395b3-2c9f-42af-ab29-f5ce752b57c9"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("aa0eccb7-b213-4838-8a2f-f48f2edd68b4"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("b0f81152-cc31-49f9-bb4d-af49750fbebe"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("b53984e1-2caa-487e-95fa-b95219c11e55"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("b69dd26d-c726-4cb1-aa94-d0e483daa8d2"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("c132c8ba-c564-4667-8b56-a9fea1fc7d24"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("db1c7baa-1307-4b12-8136-60efa358036d"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("e40f31ab-2155-4084-9710-7c6a889cf071"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("f34e8539-03b4-47f5-98be-c9ae8808f527"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("ff115094-c2f8-46a2-9cb6-b1458f9ef92e"));

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -24);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -23);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -22);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -21);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -20);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -19);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -18);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -17);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -16);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -15);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -14);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -13);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -12);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -11);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -10);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -9);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -8);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -7);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -6);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -5);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "RoleClaimId",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "89117c41-23b9-4330-ada1-57464fc84aa0");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "2ada0e62-3793-4d90-b635-80e334362e65",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Owner", "OWNER" });
        }
    }
}


