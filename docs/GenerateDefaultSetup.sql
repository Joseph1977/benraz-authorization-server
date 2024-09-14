--Generate default aplication and user
--Application: Authorization
--user name: admin@admin.com
--password: Qwerty001!

declare @ApplicationID NVARCHAR(36) = 'D5069819-1256-4AEF-9341-2628237C8EE6'
declare @Name NVARCHAR(36) = 'Authorization'
declare @Audience NVARCHAR(36) = 'Authorization'
declare @URL NVARCHAR(36) = 'http://localhost:4200/auth/callback'

INSERT INTO [dbo].[Applications]
           ([ApplicationId]
           ,[CreateTimeUtc]
           ,[UpdateTimeUtc]
           ,[Name]
           ,[Audience]
           ,[CreatedBy]
           ,[UpdatedBy]
           ,[AccessTokenCookieName])
     VALUES
           (@ApplicationID
           ,'2020-07-16 00:00:00.0000000'
           ,'2020-07-16 00:00:00.0000000'
           ,@Name
           ,@Audience
           ,NULL
           ,NULL
           ,0)


INSERT INTO [dbo].[ApplicationUrls]
           ([ApplicationUrlId]
           ,[ApplicationId]
           ,[ApplicationUrlTypeId]
           ,[Url])
     VALUES
           (NEWID()
           ,@ApplicationID
           ,2
           ,@URL)

GO

INSERT INTO [dbo].[SsoConnections]
([SsoConnectionId]
,[CreateTimeUtc]
,[UpdateTimeUtc]
,[SsoProviderId]
,[ApplicationId]
,[IsEnabled]
,[AuthorizationUrl]
,[TokenUrl]
,[ClientId]
,[ClientSecret]
,[Scope])
VALUES
(newid()
,getdate()
,getdate()
,1
,'D5069819-1256-4AEF-9341-2628237C8EE6'
,1
,null
,null
,null
,null
,null)
GO



DROP TABLE if exists ##global_var
 
CREATE TABLE ##global_var 
             (Email NVARCHAR(36)
             ,Name NVARCHAR(36))

INSERT INTO ##global_var 
    VALUES   ('admin@admin.com' -- user email
             ,'Admin benraz') -- user full name
			 
--  'Your password will be Qwerty001!'

INSERT INTO [dbo].[Users]
           ([UserId]
           ,[UserName]
           ,[NormalizedUserName]
           ,[Email]
           ,[NormalizedEmail]
           ,[EmailConfirmed]
           ,[PasswordHash]
           ,[SecurityStamp]
           ,[ConcurrencyStamp]
           ,[PhoneNumber]
           ,[PhoneNumberConfirmed]
           ,[TwoFactorEnabled]
           ,[LockoutEnd]
           ,[LockoutEnabled]
           ,[AccessFailedCount]
           ,[StatusCode]
           ,[FullName]
           ,[CreateTimeUtc])
     VALUES
           (NEWID()
           ,Lower((SELECT Email FROM ##global_var))
           ,UPPER((SELECT Email FROM ##global_var))
           ,Lower((SELECT Email FROM ##global_var))
           ,UPPER((SELECT Email FROM ##global_var))
           ,1
           ,'AQAAAAEAACcQAAAAENlwkhgAB1bFz+Sj35p6qJP+788vzyzmIH8vJSelOm91I2r7pX9Wqh0ZsqJsIzyiiw=='
           ,NULL
           ,NULL
           ,'2222222222'
           ,0
           ,0
           ,NULL
           ,0
           ,0
           ,1
           ,(SELECT Name FROM ##global_var)
           ,GETDATE())
GO


INSERT INTO [dbo].[UserRoles]
           ([UserId]
           ,[RoleId])
     VALUES
           ((SELECT [UserId] FROM [dbo].[Users] WHERE [Email] = Lower((SELECT Email FROM ##global_var)))
           ,'89117c41-23b9-4330-ada1-57464fc84aa0')
GO


UPDATE [dbo].[SettingsEntries]
   SET [Value] = '-----BEGIN RSA PRIVATE KEY-----
MIICXQIBAAKBgQDaryHFixw+giNaTf+KO20bI4MfKRyaNl7bBb+ZsumzeQ812pRa
6O28+1NBOwpqHA4CEKJskkw2dS4Tu4pqe7j7wXDXpJqVCEo2/9hi406wTPeVmrYQ
gD/q0FlKRX/odYrJuGE2Y46hqRwFfRxlKlcrhjPfHlh7WtH+5VoqNMWYxwIDAQAB
AoGARt7Q7As8OQnF2UND2JGPt2bX3KZfLZ8HOKXxRSdVU6OdCU/wGlI4kbFFdvNi
WacD5ylq6hKzfkaVizGRFxdHiD3o/rEkcDUp13ObiwsYe2HcQQZE/VkFs0lhlOzt
Ubren6nOXgm/cRpEBbWYiLdGCMFjqT4A/CXhwMQopDoAOQECQQDziAfCO2GtZfte
uwgqQF30aQnRoX7WFYeYNEu3aRUWJlAtKLU0Ao7Dv3x/uprvXTYg+XgE145kSEoV
6XNP1RdHAkEA5eFtnLCwUP7EUgVfXiTFbTuMZNU4svzuTSSXyFDRd0chcOuIveQN
eblYhy8156F/wx5Jba5dX6sAciF7w98ygQJAGISLc2yTCugHhKQD0G0miGLC0E4X
/Omx+wrYzKBRtScqT0GX7KKNSPvQRvO8gXi66Fr1UFd7SHFWtBoKt/DWJQJBAKTk
EzzenEosxGNVCTg9RgP5P9Yf/4Cb3s8k3V7JYcaeFgWqXTZgO65BXZfyLEdO15b8
xYzQEpgJz9MN43n3QoECQQCtXIiSjXZLKO3o6AbPiu8KdR9N7DvwO1I/ZgsNnUGL
gLHvB3NZ9j0b9bTqMihC9k0O3D8St56jkymauXS6iOCw
-----END RSA PRIVATE KEY-----'
Where [SettingsEntryId] = 'Jwt:PrivateKeyPem'

GO

UPDATE [dbo].[SettingsEntries]
   SET [Value] = '-----BEGIN PUBLIC KEY-----
MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDaryHFixw+giNaTf+KO20bI4Mf
KRyaNl7bBb+ZsumzeQ812pRa6O28+1NBOwpqHA4CEKJskkw2dS4Tu4pqe7j7wXDX
pJqVCEo2/9hi406wTPeVmrYQgD/q0FlKRX/odYrJuGE2Y46hqRwFfRxlKlcrhjPf
Hlh7WtH+5VoqNMWYxwIDAQAB
-----END PUBLIC KEY-----'
WHERE [SettingsEntryId] = 'Jwt:PublicKeyPem'
    
GO
