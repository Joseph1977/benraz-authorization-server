USE [Benraz.Authorization]
GO

Select * from Claims where Value like 'authorization%'

DECLARE @_ClaimId uniqueidentifier = NEWID()
DECLARE @_CreateTimeUtc datetime2(7) = GETUTCDATE()
DECLARE @_UpdateTimeUtc datetime2(7)= GETUTCDATE()
DECLARE @_Type nvarchar(max) = 'claim'
DECLARE @_Value nvarchar(max)
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-application-read'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-application-add'  
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-application-update'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-application-delete'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-user-read'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-user-read-one'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-user-add'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-user-update'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-user-delete'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-user-status-read'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-user-status-suspend'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-user-status-block'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-user-role-read'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-user-role-update'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-user-claim-read'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-user-claim-update'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-user-email-read'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-user-email-update'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-user-email-verify'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-user-phone-read'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-user-phone-verify'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-user-password-update'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-user-password-reset'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-user-unlock'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-employee-read'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-role-read'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-role-add'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-role-update'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-role-delete'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-claim-read'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-claim-add'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-claim-delete'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-profile-password-change'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-profile-password-set'
set @_ClaimId = NEWID() set  @_CreateTimeUtc = GETUTCDATE() set @_UpdateTimeUtc = GETUTCDATE() EXECUTE [dbo].[RoleClaim] @_ClaimId ,@_CreateTimeUtc ,@_UpdateTimeUtc ,@_Type, N'authorization-token-exchange'


