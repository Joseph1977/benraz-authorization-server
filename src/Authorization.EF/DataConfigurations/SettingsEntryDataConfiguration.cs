using Authorization.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Benraz.Infrastructure.Emails;

namespace Authorization.EF.DataConfigurations
{
    class SettingsEntryDataConfiguration : IEntityTypeConfiguration<SettingsEntry>
    {
        public void Configure(EntityTypeBuilder<SettingsEntry> builder)
        {
            builder.HasData(
                Create("MicrosoftGraph:BaseUrl", "https://graph.microsoft.com/v1.0/"),
                Create("MicrosoftGraph:ProfileEndpoint", "me"),
                Create("MicrosoftGraph:MemberGroupsEndpoint", "me/getMemberGroups"),
                Create("MicrosoftGraph:MemberOfEndpoint", "me/memberOf"),
                Create("MicrosoftGraph:TransitiveMemberOfEndpoint", "me/transitiveMemberOf"),
                Create("MicrosoftGraph:GroupsEndpoint", "groups"),
                Create("FacebookGraphGateway:BaseUrl", "https://graph.facebook.com/"),
                Create("Jwt:Issuer", "Benraz Authorization Server"),
                Create("Jwt:ValidityPeriod", "1.00:00:00"),
                Create("Jwt:PasswordExpiredValidityPeriod", "00:05:00"),
                Create("Jwt:PasswordExpiredAudience", "Authorization"),
                Create("Jwt:SetPasswordValidityPeriod", "1.00:00:00"),
                Create("Jwt:ConfirmEmailValidityPeriod", "1.00:00:00"),
                Create("Jwt:SetPasswordAudience", "Authorization"),
                Create("Jwt:PrivateKeyPem", null),
                Create("Jwt:PublicKeyPem", null),
                Create("UserPasswords:KeepUserPasswordsCount", 5),
                Create("UserPasswords:MaxAccessFailedCount", 6),
                Create("UserPasswords:LockoutPeriod", "00:30:00"),
                Create("UserPasswords:PasswordExpirationPeriod", null),
                Create("Authorization:AuthorizeUnconfirmedEmailPeriod", "00:00:00"),
                Create("Authorization:MfaCodeLength", 8, "Mfa code length"),
                Create("Authorization:AccessTokenMfaCodeLength", 10, "Access token code length"),
                Create("Authorization:IsMfaEnabled", false, "Enable mfa two factor service or not"),
                Create("Emails:From", "info@benraz.com"),
                Create("Emails:FromDisplayName", "Benraz"),
                Create("Emails:CompanyName", "Benraz"),
                Create("Emails:CompanyLogoUrl", "https://www.benraz.com/wp-content/themes/benraz-by-dorzki/assets/images/logo.svg"),
                Create("Emails:CompanyEmail", "support@benraz.com"),
                Create("Emails:CompanyPhone", "1-888-497-5499"),
                Create("Emails:ConfirmEmailSubject", "Email confirmation", "Confirmation email subject"),
                Create("Emails:ResetPasswordSubject", "Reset password", "Reset password email subject"),
                Create("Emails:MfaCodeSubject", "Your BENRAZ {0} code", "Mfa code email subject and replacing the {0} by action type"),
                Create("Emails:ServiceProvider:ServiceType", (int)EmailsServiceType.Benraz, "Email service provider type; 1 - Benraz services"),
                Create("Emails:ServiceProvider:Benraz:TemplateId", "c7a628da-d901-4916-9618-984eeaa28fda", "Benraz email services default template identifier"),
                Create("Emails:ServiceProvider:Benraz:AccessToken", null, "Benraz email services access token"),
                Create("Emails:ServiceProvider:Benraz:BaseUrl", null, "Benraz email services base URL"),
                Create("User:DefaultRolesList", null, "Default roles list."),
                Create("User:VerificationCodeCooldown", "00:01:00", "Verification code cooldown."),
                Create("UserActionNotifications:ReceiversEmails", null, "Receivers emails can be list of emails divided by ';'."),
                Create("UserActionNotifications:IsLoginNotifyEnabled", false, "Enable send user login email service or not"),
                Create("UserActionNotifications:UserLoginEmailSubject", "User logged", "User login email subject"));
        }

        private SettingsEntry Create(string id, object value, string description = null)
        {
            return new SettingsEntry
            {
                Id = id,
                Value = value?.ToString(),
                Description = description
            };
        }
    }
}


