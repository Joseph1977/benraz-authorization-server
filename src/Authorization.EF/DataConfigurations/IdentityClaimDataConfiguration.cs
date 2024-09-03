using Authorization.Domain.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Authorization.EF.DataConfigurations
{
    class IdentityClaimDataConfiguration : IEntityTypeConfiguration<IdentityClaim>
    {
        public void Configure(EntityTypeBuilder<IdentityClaim> builder)
        {
            builder.HasData(
                Create("418270bb-d7ad-435b-8c8e-092d27da81b5", "claim", "authorization-application-read", "2020-02-29", "2020-02-29"),
                Create("3e151682-a45b-4459-815b-36be325fdf59", "claim", "authorization-application-add", "2020-02-29", "2020-02-29"),
                Create("49a2e542-d6da-4c7c-b2d7-13720b03619b", "claim", "authorization-application-update", "2020-02-29", "2020-02-29"),
                Create("b0f81152-cc31-49f9-bb4d-af49750fbebe", "claim", "authorization-application-delete", "2020-02-29", "2020-02-29"),
                Create("613f8d53-d4a9-4d7d-afc7-72190b8c6adb", "claim", "authorization-user-read", "2020-02-29", "2020-02-29"),
                Create("28aa11a0-6544-4bb2-96bd-6a2a98aebcdd", "claim", "authorization-user-add", "2020-03-05", "2020-03-05"),
                Create("07ab8a18-fe49-489f-b772-0213d6abacc0", "claim", "authorization-user-update", "2020-02-29", "2020-02-29"),
                Create("f34e8539-03b4-47f5-98be-c9ae8808f527", "claim", "authorization-user-delete", "2020-02-29", "2020-02-29"),
                Create("b69dd26d-c726-4cb1-aa94-d0e483daa8d2", "claim", "authorization-user-status-read", "2020-02-29", "2020-02-29"),
                Create("2cb96ac2-a8d0-44ac-8b8f-4e344e77345a", "claim", "authorization-user-status-suspend", "2020-02-29", "2020-02-29"),
                Create("db1c7baa-1307-4b12-8136-60efa358036d", "claim", "authorization-user-status-block", "2020-02-29", "2020-02-29"),
                Create("c132c8ba-c564-4667-8b56-a9fea1fc7d24", "claim", "authorization-user-role-read", "2020-02-29", "2020-02-29"),
                Create("ff115094-c2f8-46a2-9cb6-b1458f9ef92e", "claim", "authorization-user-role-update", "2020-02-29", "2020-02-29"),
                Create("e40f31ab-2155-4084-9710-7c6a889cf071", "claim", "authorization-user-claim-read", "2020-02-29", "2020-02-29"),
                Create("6c9eb24a-39cd-4917-8483-08d7baf6949a", "claim", "authorization-user-claim-update", "2020-02-29", "2020-02-29"),
                Create("e837054c-4ad2-4792-ac9a-e45f91e5ddb3", "claim", "authorization-user-email-read", "2020-03-04", "2020-03-04"),
                Create("a55395b3-2c9f-42af-ab29-f5ce752b57c9", "claim", "authorization-user-email-verify", "2020-02-29", "2020-02-29"),
                Create("f7958c48-3473-493e-902b-e99a2b930518", "claim", "authorization-user-phone-read", "2020-03-04", "2020-03-04"),
                Create("461469ab-6a88-4e01-8b9c-b50bc41f5c47", "claim", "authorization-user-phone-verify", "2020-02-29", "2020-02-29"),
                Create("75baa2b9-b8d7-4d1c-a41c-e6cbc6a0d81f", "claim", "authorization-user-password-reset", "2020-02-29", "2020-02-29"),
                Create("0f5e1f02-75df-4cc0-80b3-79072f8c1000", "claim", "authorization-role-read", "2020-02-29", "2020-02-29"),
                Create("aa0eccb7-b213-4838-8a2f-f48f2edd68b4", "claim", "authorization-role-add", "2020-02-29", "2020-02-29"),
                Create("01af747a-c712-4c28-a95b-c7fd48434a47", "claim", "authorization-role-update", "2020-02-29", "2020-02-29"),
                Create("2a1d29ea-60ce-4358-ac90-0e16a6fd2b96", "claim", "authorization-role-delete", "2020-02-29", "2020-02-29"),
                Create("21310d21-7b58-478f-96a0-8cf35dbb95c0", "claim", "authorization-claim-read", "2020-02-29", "2020-02-29"),
                Create("b53984e1-2caa-487e-95fa-b95219c11e55", "claim", "authorization-claim-add", "2020-02-29", "2020-02-29"),
                Create("1a71b60b-7baf-47f6-8c08-950148b9fc43", "claim", "authorization-claim-delete", "2020-02-29", "2020-02-29"),
                Create("b41fa3c7-3d0a-405e-aa46-da6060bca866", "claim", "erpmaintenance-category-read", "2020-03-13", "2020-03-13"),
                Create("e402e793-4c50-4f57-a396-6a724a84779f", "claim", "erpmaintenance-priority-read", "2020-03-13", "2020-03-13"),
                Create("bfd021cb-ddb8-4158-9adf-51e29d0fe27d", "claim", "erpmaintenance-status-read", "2020-03-13", "2020-03-13"),
                Create("6dc448af-7ba0-4440-97f7-c92577e9d408", "claim", "erpmaintenance-provider-read", "2020-03-13", "2020-03-13"),
                Create("fb36a55f-1b7b-46e3-bb88-2fd9af737a5c", "claim", "erpmaintenance-provider-add", "2020-03-13", "2020-03-13"),
                Create("c3ec6edb-b722-4f8d-af94-4c37cfc4fe30", "claim", "erpmaintenance-provider-update", "2020-03-13", "2020-03-13"),
                Create("446d3a60-d259-4ba2-b6f2-4f3b8c9b0ced", "claim", "erpmaintenance-provider-delete", "2020-03-13", "2020-03-13"),
                Create("63114fdf-9d2f-4e35-af6e-995938dbef55", "claim", "erpmaintenance-client-read", "2020-03-13", "2020-03-13"),
                Create("428b603b-9d08-48ff-bcc9-f2c19d5e1e81", "claim", "erpmaintenance-client-add", "2020-03-13", "2020-03-13"),
                Create("b3d5d053-8b4b-4a67-9691-ce9727bf7753", "claim", "erpmaintenance-client-update", "2020-03-13", "2020-03-13"),
                Create("e1ea0fd3-661d-413a-b625-06d07f1acb7f", "claim", "erpmaintenance-client-delete", "2020-03-13", "2020-03-13"),
                Create("0c3c76e1-6ee4-4715-987d-ac094ef8e81b", "claim", "erpmaintenance-integration-read", "2020-03-13", "2020-03-13"),
                Create("38184f60-d83a-4735-a095-73eb4b7984c9", "claim", "erpmaintenance-integration-add", "2020-03-13", "2020-03-13"),
                Create("a3dd97eb-0cff-4015-83e4-92228335e36b", "claim", "erpmaintenance-integration-update", "2020-03-13", "2020-03-13"),
                Create("ee9649ec-5bc4-4820-aa4f-2e343184ce94", "claim", "erpmaintenance-integration-delete", "2020-03-13", "2020-03-13"),
                Create("01ec1b7e-a09f-4a1a-b601-1adbf9a97a12", "claim", "erpmaintenance-job-read", "2020-03-13", "2020-03-13"),
                Create("1578c036-f217-4f90-855c-479150e553a1", "claim", "erpmaintenance-job-executesynchronization", "2020-03-13", "2020-03-13"),
                Create("1ea02e47-ab70-452a-8d0f-e6a1509f2296", "claim", "erpmaintenance-maintenanceissue-read", "2020-03-13", "2020-03-13"));
        }

        private IdentityClaim Create(string id, string type, string value, string createTimeUtc, string updateTimeUtc)
        {
            return new IdentityClaim
            {
                Id = Guid.Parse(id),
                Type = type,
                Value = value,
                CreateTimeUtc = DateTime.Parse(createTimeUtc),
                UpdateTimeUtc = DateTime.Parse(updateTimeUtc)
            };
        }
    }
}


