using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Authorization.EF.Migrations
{
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    ApplicationId = table.Column<Guid>(nullable: false),
                    CreateTimeUtc = table.Column<DateTime>(nullable: false),
                    UpdateTimeUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    Audience = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.ApplicationId);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUrlTypes",
                columns: table => new
                {
                    ApplicationUrlTypeId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUrlTypes", x => x.ApplicationUrlTypeId);
                });

            migrationBuilder.CreateTable(
                name: "SettingsEntries",
                columns: table => new
                {
                    SettingsEntryId = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingsEntries", x => x.SettingsEntryId);
                });

            migrationBuilder.CreateTable(
                name: "SsoProviders",
                columns: table => new
                {
                    SsoProviderId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SsoProviders", x => x.SsoProviderId);
                });

            migrationBuilder.CreateTable(
                name: "UsageLogs",
                columns: table => new
                {
                    UsageLogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateTimeUtc = table.Column<DateTime>(nullable: false),
                    UpdateTimeUtc = table.Column<DateTime>(nullable: false),
                    UserName = table.Column<string>(maxLength: 500, nullable: true),
                    IPAddress = table.Column<string>(maxLength: 100, nullable: true),
                    Action = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsageLogs", x => x.UsageLogId);
                });

            migrationBuilder.CreateTable(
                name: "UserClaimMappingEntries",
                columns: table => new
                {
                    UserClaimMappingEntryId = table.Column<Guid>(nullable: false),
                    CreateTimeUtc = table.Column<DateTime>(nullable: false),
                    UpdateTimeUtc = table.Column<DateTime>(nullable: false),
                    SourceType = table.Column<string>(nullable: true),
                    SourceValue = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaimMappingEntries", x => x.UserClaimMappingEntryId);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUrls",
                columns: table => new
                {
                    ApplicationUrlId = table.Column<Guid>(nullable: false),
                    ApplicationId = table.Column<Guid>(nullable: false),
                    ApplicationUrlTypeId = table.Column<int>(nullable: false),
                    Url = table.Column<string>(type: "nvarchar(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUrls", x => x.ApplicationUrlId);
                    table.ForeignKey(
                        name: "FK_ApplicationUrls_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "ApplicationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUrls_ApplicationUrlTypes_ApplicationUrlTypeId",
                        column: x => x.ApplicationUrlTypeId,
                        principalTable: "ApplicationUrlTypes",
                        principalColumn: "ApplicationUrlTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SsoConnections",
                columns: table => new
                {
                    SsoConnectionId = table.Column<Guid>(nullable: false),
                    CreateTimeUtc = table.Column<DateTime>(nullable: false),
                    UpdateTimeUtc = table.Column<DateTime>(nullable: false),
                    SsoProviderId = table.Column<int>(nullable: false),
                    ApplicationId = table.Column<Guid>(nullable: true),
                    IsEnabled = table.Column<bool>(nullable: false),
                    AuthorizationUrl = table.Column<string>(type: "nvarchar(2000)", nullable: true),
                    TokenUrl = table.Column<string>(type: "nvarchar(2000)", nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    ClientSecret = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    Scope = table.Column<string>(type: "nvarchar(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SsoConnections", x => x.SsoConnectionId);
                    table.ForeignKey(
                        name: "FK_SsoConnections_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "ApplicationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SsoConnections_SsoProviders_SsoProviderId",
                        column: x => x.SsoProviderId,
                        principalTable: "SsoProviders",
                        principalColumn: "SsoProviderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ApplicationUrlTypes",
                columns: new[] { "ApplicationUrlTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Claims URL" },
                    { 2, "Authorization callback URL" }
                });

            migrationBuilder.InsertData(
                table: "SettingsEntries",
                columns: new[] { "SettingsEntryId", "Value" },
                values: new object[,]
                {
                    { "Jwt:PublicKeyPem", null },
                    { "Jwt:PrivateKeyPem", null },
                    { "Jwt:ValidityPeriod", "1.00:00:00" },
                    { "Jwt:Issuer", "Benraz Authorization Server" },
                    { "MicrosoftGraph:GroupsEndpoint", "groups" },
                    { "MicrosoftGraph:MemberOfEndpoint", "me/memberOf" },
                    { "MicrosoftGraph:MemberGroupsEndpoint", "me/getMemberGroups" },
                    { "MicrosoftGraph:ProfileEndpoint", "me" },
                    { "MicrosoftGraph:BaseUrl", "https://graph.microsoft.com/v1.0/" },
                    { "MicrosoftGraph:TransitiveMemberOfEndpoint", "me/transitiveMemberOf" }
                });

            migrationBuilder.InsertData(
                table: "SsoProviders",
                columns: new[] { "SsoProviderId", "Name" },
                values: new object[,]
                {
                    { 1, "Internal" },
                    { 2, "Microsoft" },
                    { 3, "Facebook" }
                });

            migrationBuilder.InsertData(
                table: "UserClaimMappingEntries",
                columns: new[] { "UserClaimMappingEntryId", "CreateTimeUtc", "SourceType", "SourceValue", "Type", "UpdateTimeUtc", "Value" },
                values: new object[,]
                {
                    { new Guid("dd19cb61-0bd8-4af0-a574-724ae3c9e28f"), new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "groups", "AuthorizationServer-Write", "roles", new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Write" },
                    { new Guid("15f3d112-5154-485b-b7d3-c83e50da08bb"), new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "groups", "AuthorizationServer-Read", "roles", new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Read" },
                    { new Guid("65f9f76f-1122-4b9b-9764-740feaa5b269"), new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "groups", "AuthorizationServer-Admin", "roles", new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Administrator" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUrls_ApplicationId",
                table: "ApplicationUrls",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUrls_ApplicationUrlTypeId",
                table: "ApplicationUrls",
                column: "ApplicationUrlTypeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SsoConnections_ApplicationId",
                table: "SsoConnections",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_SsoConnections_SsoProviderId",
                table: "SsoConnections",
                column: "SsoProviderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUrls");

            migrationBuilder.DropTable(
                name: "SettingsEntries");

            migrationBuilder.DropTable(
                name: "SsoConnections");

            migrationBuilder.DropTable(
                name: "UsageLogs");

            migrationBuilder.DropTable(
                name: "UserClaimMappingEntries");

            migrationBuilder.DropTable(
                name: "ApplicationUrlTypes");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "SsoProviders");
        }
    }
}


