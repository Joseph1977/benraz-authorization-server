IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Applications] (
    [ApplicationId] uniqueidentifier NOT NULL,
    [CreateTimeUtc] datetime2 NOT NULL,
    [UpdateTimeUtc] datetime2 NOT NULL,
    [Name] nvarchar(500) NULL,
    [Audience] nvarchar(max) NULL,
    [CreatedBy] nvarchar(100) NULL,
    [UpdatedBy] nvarchar(100) NULL,
    CONSTRAINT [PK_Applications] PRIMARY KEY ([ApplicationId])
);

GO

CREATE TABLE [ApplicationUrlTypes] (
    [ApplicationUrlTypeId] int NOT NULL,
    [Name] nvarchar(500) NULL,
    CONSTRAINT [PK_ApplicationUrlTypes] PRIMARY KEY ([ApplicationUrlTypeId])
);

GO

CREATE TABLE [SettingsEntries] (
    [SettingsEntryId] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_SettingsEntries] PRIMARY KEY ([SettingsEntryId])
);

GO

CREATE TABLE [SsoProviders] (
    [SsoProviderId] int NOT NULL,
    [Name] nvarchar(500) NULL,
    CONSTRAINT [PK_SsoProviders] PRIMARY KEY ([SsoProviderId])
);

GO

CREATE TABLE [UsageLogs] (
    [UsageLogId] int NOT NULL IDENTITY,
    [CreateTimeUtc] datetime2 NOT NULL,
    [UpdateTimeUtc] datetime2 NOT NULL,
    [UserName] nvarchar(500) NULL,
    [IPAddress] nvarchar(100) NULL,
    [Action] nvarchar(2000) NULL,
    CONSTRAINT [PK_UsageLogs] PRIMARY KEY ([UsageLogId])
);

GO

CREATE TABLE [UserClaimMappingEntries] (
    [UserClaimMappingEntryId] uniqueidentifier NOT NULL,
    [CreateTimeUtc] datetime2 NOT NULL,
    [UpdateTimeUtc] datetime2 NOT NULL,
    [SourceType] nvarchar(max) NULL,
    [SourceValue] nvarchar(max) NULL,
    [Type] nvarchar(max) NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_UserClaimMappingEntries] PRIMARY KEY ([UserClaimMappingEntryId])
);

GO

CREATE TABLE [ApplicationUrls] (
    [ApplicationUrlId] uniqueidentifier NOT NULL,
    [ApplicationId] uniqueidentifier NOT NULL,
    [ApplicationUrlTypeId] int NOT NULL,
    [Url] nvarchar(2000) NULL,
    CONSTRAINT [PK_ApplicationUrls] PRIMARY KEY ([ApplicationUrlId]),
    CONSTRAINT [FK_ApplicationUrls_Applications_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [Applications] ([ApplicationId]) ON DELETE CASCADE,
    CONSTRAINT [FK_ApplicationUrls_ApplicationUrlTypes_ApplicationUrlTypeId] FOREIGN KEY ([ApplicationUrlTypeId]) REFERENCES [ApplicationUrlTypes] ([ApplicationUrlTypeId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [SsoConnections] (
    [SsoConnectionId] uniqueidentifier NOT NULL,
    [CreateTimeUtc] datetime2 NOT NULL,
    [UpdateTimeUtc] datetime2 NOT NULL,
    [SsoProviderId] int NOT NULL,
    [ApplicationId] uniqueidentifier NULL,
    [IsEnabled] bit NOT NULL,
    [AuthorizationUrl] nvarchar(2000) NULL,
    [TokenUrl] nvarchar(2000) NULL,
    [ClientId] nvarchar(500) NULL,
    [ClientSecret] nvarchar(500) NULL,
    [Scope] nvarchar(2000) NULL,
    CONSTRAINT [PK_SsoConnections] PRIMARY KEY ([SsoConnectionId]),
    CONSTRAINT [FK_SsoConnections_Applications_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [Applications] ([ApplicationId]) ON DELETE CASCADE,
    CONSTRAINT [FK_SsoConnections_SsoProviders_SsoProviderId] FOREIGN KEY ([SsoProviderId]) REFERENCES [SsoProviders] ([SsoProviderId]) ON DELETE NO ACTION
);

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ApplicationUrlTypeId', N'Name') AND [object_id] = OBJECT_ID(N'[ApplicationUrlTypes]'))
    SET IDENTITY_INSERT [ApplicationUrlTypes] ON;
INSERT INTO [ApplicationUrlTypes] ([ApplicationUrlTypeId], [Name])
VALUES (1, N'Claims URL'),
(2, N'Authorization callback URL');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ApplicationUrlTypeId', N'Name') AND [object_id] = OBJECT_ID(N'[ApplicationUrlTypes]'))
    SET IDENTITY_INSERT [ApplicationUrlTypes] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'SettingsEntryId', N'Value') AND [object_id] = OBJECT_ID(N'[SettingsEntries]'))
    SET IDENTITY_INSERT [SettingsEntries] ON;
INSERT INTO [SettingsEntries] ([SettingsEntryId], [Value])
VALUES (N'Jwt:PublicKeyPem', NULL),
(N'Jwt:PrivateKeyPem', NULL),
(N'Jwt:ValidityPeriod', N'1.00:00:00'),
(N'Jwt:Issuer', N'Benraz Authorization Server'),
(N'MicrosoftGraph:GroupsEndpoint', N'groups'),
(N'MicrosoftGraph:MemberOfEndpoint', N'me/memberOf'),
(N'MicrosoftGraph:MemberGroupsEndpoint', N'me/getMemberGroups'),
(N'MicrosoftGraph:ProfileEndpoint', N'me'),
(N'MicrosoftGraph:BaseUrl', N'https://graph.microsoft.com/v1.0/'),
(N'MicrosoftGraph:TransitiveMemberOfEndpoint', N'me/transitiveMemberOf');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'SettingsEntryId', N'Value') AND [object_id] = OBJECT_ID(N'[SettingsEntries]'))
    SET IDENTITY_INSERT [SettingsEntries] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'SsoProviderId', N'Name') AND [object_id] = OBJECT_ID(N'[SsoProviders]'))
    SET IDENTITY_INSERT [SsoProviders] ON;
INSERT INTO [SsoProviders] ([SsoProviderId], [Name])
VALUES (1, N'Internal'),
(2, N'Microsoft'),
(3, N'Facebook');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'SsoProviderId', N'Name') AND [object_id] = OBJECT_ID(N'[SsoProviders]'))
    SET IDENTITY_INSERT [SsoProviders] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'UserClaimMappingEntryId', N'CreateTimeUtc', N'SourceType', N'SourceValue', N'Type', N'UpdateTimeUtc', N'Value') AND [object_id] = OBJECT_ID(N'[UserClaimMappingEntries]'))
    SET IDENTITY_INSERT [UserClaimMappingEntries] ON;
INSERT INTO [UserClaimMappingEntries] ([UserClaimMappingEntryId], [CreateTimeUtc], [SourceType], [SourceValue], [Type], [UpdateTimeUtc], [Value])
VALUES ('dd19cb61-0bd8-4af0-a574-724ae3c9e28f', '2020-02-10T00:00:00.0000000Z', N'groups', N'AuthorizationServer-Write', N'roles', '2020-02-10T00:00:00.0000000Z', N'Write'),
('15f3d112-5154-485b-b7d3-c83e50da08bb', '2020-02-10T00:00:00.0000000Z', N'groups', N'AuthorizationServer-Read', N'roles', '2020-02-10T00:00:00.0000000Z', N'Read'),
('65f9f76f-1122-4b9b-9764-740feaa5b269', '2020-02-10T00:00:00.0000000Z', N'groups', N'AuthorizationServer-Admin', N'roles', '2020-02-10T00:00:00.0000000Z', N'Administrator');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'UserClaimMappingEntryId', N'CreateTimeUtc', N'SourceType', N'SourceValue', N'Type', N'UpdateTimeUtc', N'Value') AND [object_id] = OBJECT_ID(N'[UserClaimMappingEntries]'))
    SET IDENTITY_INSERT [UserClaimMappingEntries] OFF;

GO

CREATE INDEX [IX_ApplicationUrls_ApplicationId] ON [ApplicationUrls] ([ApplicationId]);

GO

CREATE UNIQUE INDEX [IX_ApplicationUrls_ApplicationUrlTypeId] ON [ApplicationUrls] ([ApplicationUrlTypeId]);

GO

CREATE INDEX [IX_SsoConnections_ApplicationId] ON [SsoConnections] ([ApplicationId]);

GO

CREATE INDEX [IX_SsoConnections_SsoProviderId] ON [SsoConnections] ([SsoProviderId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200211224249_CreateDatabase', N'2.2.6-servicing-10079');

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'SsoProviderId', N'Name') AND [object_id] = OBJECT_ID(N'[SsoProviders]'))
    SET IDENTITY_INSERT [SsoProviders] ON;
INSERT INTO [SsoProviders] ([SsoProviderId], [Name])
VALUES (4, N'Google');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'SsoProviderId', N'Name') AND [object_id] = OBJECT_ID(N'[SsoProviders]'))
    SET IDENTITY_INSERT [SsoProviders] OFF;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200213170207_InsertGoogleSsoProvider', N'2.2.6-servicing-10079');

GO

CREATE TABLE [Roles] (
    [RoleId] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([RoleId])
);

GO

CREATE TABLE [UserStatuses] (
    [UserStatusId] int NOT NULL,
    [Name] nvarchar(500) NULL,
    CONSTRAINT [PK_UserStatuses] PRIMARY KEY ([UserStatusId])
);

GO

CREATE TABLE [RoleClaims] (
    [RoleClaimId] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_RoleClaims] PRIMARY KEY ([RoleClaimId]),
    CONSTRAINT [FK_RoleClaims_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([RoleId]) ON DELETE CASCADE
);

GO

CREATE TABLE [Users] (
    [UserId] nvarchar(450) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    [StatusCode] int NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserId]),
    CONSTRAINT [FK_Users_UserStatuses_StatusCode] FOREIGN KEY ([StatusCode]) REFERENCES [UserStatuses] ([UserStatusId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [UserClaims] (
    [UserClaimId] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_UserClaims] PRIMARY KEY ([UserClaimId]),
    CONSTRAINT [FK_UserClaims_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
);

GO

CREATE TABLE [UserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_UserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_UserLogins_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
);

GO

CREATE TABLE [UserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([RoleId]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
);

GO

CREATE TABLE [UserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_UserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_UserTokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
);

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'UserStatusId', N'Name') AND [object_id] = OBJECT_ID(N'[UserStatuses]'))
    SET IDENTITY_INSERT [UserStatuses] ON;
INSERT INTO [UserStatuses] ([UserStatusId], [Name])
VALUES (1, N'Active'),
(2, N'Suspended'),
(3, N'Blocked'),
(4, N'Payment service suspended');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'UserStatusId', N'Name') AND [object_id] = OBJECT_ID(N'[UserStatuses]'))
    SET IDENTITY_INSERT [UserStatuses] OFF;

GO

CREATE INDEX [IX_RoleClaims_RoleId] ON [RoleClaims] ([RoleId]);

GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [Roles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

GO

CREATE INDEX [IX_UserClaims_UserId] ON [UserClaims] ([UserId]);

GO

CREATE INDEX [IX_UserLogins_UserId] ON [UserLogins] ([UserId]);

GO

CREATE INDEX [IX_UserRoles_RoleId] ON [UserRoles] ([RoleId]);

GO

CREATE INDEX [IX_Users_StatusCode] ON [Users] ([StatusCode]);

GO

CREATE INDEX [EmailIndex] ON [Users] ([NormalizedEmail]);

GO

CREATE UNIQUE INDEX [UserNameIndex] ON [Users] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200218205107_AddIdentity', N'2.2.6-servicing-10079');

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[Roles]'))
    SET IDENTITY_INSERT [Roles] ON;
INSERT INTO [Roles] ([RoleId], [ConcurrencyStamp], [Name], [NormalizedName])
VALUES (N'd7b4f6d4-9cab-49ce-abf2-c0bc6d5d1ae2', N'96fd1b42-0720-4de0-be3e-d51b5bb3476d', N'Employee', N'EMPLOYEE'),
(N'd14a3723-2ca1-49b2-9d58-50220836053d', N'b1cc36c5-7958-450d-9363-70d81b4f02ab', N'Organization user', N'ORGANIZATION USER'),
(N'48aa65a2-dc1c-4ce9-bf07-ee4c05973e58', N'bbebc5db-8168-4ad1-8581-7d5e5d03d90f', N'Service provider', N'SERVICE PROVIDER'),
(N'2ada0e62-3793-4d90-b635-80e334362e65', N'2e0b2011-c2f6-4837-8aa8-ff5c1aa79201', N'Owner', N'OWNER'),
(N'c5b42e20-e5b0-4f5c-a968-dd0050e21070', N'2162f58e-8faf-4d89-9d1d-3feda66a0ee4', N'Partner', N'PARTNER'),
(N'042858f3-df80-41a9-b2f3-eff9a4a50ba9', N'6e07524f-99da-47e4-9ea0-e510de7d0d55', N'Internal server', N'INTERNAL SERVER'),
(N'e3ffc4ce-4dc1-4ea4-91c4-7939fd9a2620', N'54267f41-15fe-447c-896a-1cdd702d7ac2', N'External server', N'EXTERNAL SERVER');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[Roles]'))
    SET IDENTITY_INSERT [Roles] OFF;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200218213731_InsertIdentityRoles', N'2.2.6-servicing-10079');

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'Discriminator');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Users] DROP COLUMN [Discriminator];

GO

DROP INDEX [IX_Users_StatusCode] ON [Users];
DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'StatusCode');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Users] ALTER COLUMN [StatusCode] int NOT NULL;
CREATE INDEX [IX_Users_StatusCode] ON [Users] ([StatusCode]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200220103908_ChangeUsers', N'2.2.6-servicing-10079');

GO

DROP TABLE [UserClaimMappingEntries];

GO

DELETE FROM [ApplicationUrls]
WHERE [ApplicationUrlTypeId] = 1;
SELECT @@ROWCOUNT;


GO

DELETE FROM [ApplicationUrlTypes]
WHERE [ApplicationUrlTypeId] = 1;
SELECT @@ROWCOUNT;


GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200220125605_RemoveUserClaimMappingEntries', N'2.2.6-servicing-10079');

GO




