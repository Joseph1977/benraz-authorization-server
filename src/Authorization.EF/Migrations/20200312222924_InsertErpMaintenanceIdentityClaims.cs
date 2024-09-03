using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Authorization.EF.Migrations
{
    public partial class InsertErpMaintenanceIdentityClaims : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Claims",
                columns: new[] { "ClaimId", "CreateTimeUtc", "Type", "UpdateTimeUtc", "Value" },
                values: new object[,]
                {
                    { new Guid("b41fa3c7-3d0a-405e-aa46-da6060bca866"), new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "erpmaintenance-category-read" },
                    { new Guid("01ec1b7e-a09f-4a1a-b601-1adbf9a97a12"), new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "erpmaintenance-job-read" },
                    { new Guid("ee9649ec-5bc4-4820-aa4f-2e343184ce94"), new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "erpmaintenance-integration-delete" },
                    { new Guid("a3dd97eb-0cff-4015-83e4-92228335e36b"), new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "erpmaintenance-integration-update" },
                    { new Guid("38184f60-d83a-4735-a095-73eb4b7984c9"), new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "erpmaintenance-integration-add" },
                    { new Guid("0c3c76e1-6ee4-4715-987d-ac094ef8e81b"), new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "erpmaintenance-integration-read" },
                    { new Guid("e1ea0fd3-661d-413a-b625-06d07f1acb7f"), new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "erpmaintenance-client-delete" },
                    { new Guid("b3d5d053-8b4b-4a67-9691-ce9727bf7753"), new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "erpmaintenance-client-update" },
                    { new Guid("428b603b-9d08-48ff-bcc9-f2c19d5e1e81"), new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "erpmaintenance-client-add" },
                    { new Guid("63114fdf-9d2f-4e35-af6e-995938dbef55"), new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "erpmaintenance-client-read" },
                    { new Guid("446d3a60-d259-4ba2-b6f2-4f3b8c9b0ced"), new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "erpmaintenance-provider-delete" },
                    { new Guid("c3ec6edb-b722-4f8d-af94-4c37cfc4fe30"), new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "erpmaintenance-provider-update" },
                    { new Guid("fb36a55f-1b7b-46e3-bb88-2fd9af737a5c"), new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "erpmaintenance-provider-add" },
                    { new Guid("6dc448af-7ba0-4440-97f7-c92577e9d408"), new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "erpmaintenance-provider-read" },
                    { new Guid("bfd021cb-ddb8-4158-9adf-51e29d0fe27d"), new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "erpmaintenance-status-read" },
                    { new Guid("e402e793-4c50-4f57-a396-6a724a84779f"), new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "erpmaintenance-priority-read" },
                    { new Guid("1578c036-f217-4f90-855c-479150e553a1"), new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "erpmaintenance-job-executesynchronization" },
                    { new Guid("1ea02e47-ab70-452a-8d0f-e6a1509f2296"), new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "claim", new DateTime(2020, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "erpmaintenance-maintenanceissue-read" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("01ec1b7e-a09f-4a1a-b601-1adbf9a97a12"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("0c3c76e1-6ee4-4715-987d-ac094ef8e81b"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("1578c036-f217-4f90-855c-479150e553a1"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("1ea02e47-ab70-452a-8d0f-e6a1509f2296"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("38184f60-d83a-4735-a095-73eb4b7984c9"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("428b603b-9d08-48ff-bcc9-f2c19d5e1e81"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("446d3a60-d259-4ba2-b6f2-4f3b8c9b0ced"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("63114fdf-9d2f-4e35-af6e-995938dbef55"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("6dc448af-7ba0-4440-97f7-c92577e9d408"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("a3dd97eb-0cff-4015-83e4-92228335e36b"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("b3d5d053-8b4b-4a67-9691-ce9727bf7753"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("b41fa3c7-3d0a-405e-aa46-da6060bca866"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("bfd021cb-ddb8-4158-9adf-51e29d0fe27d"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("c3ec6edb-b722-4f8d-af94-4c37cfc4fe30"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("e1ea0fd3-661d-413a-b625-06d07f1acb7f"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("e402e793-4c50-4f57-a396-6a724a84779f"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("ee9649ec-5bc4-4820-aa4f-2e343184ce94"));

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimId",
                keyValue: new Guid("fb36a55f-1b7b-46e3-bb88-2fd9af737a5c"));
        }
    }
}


