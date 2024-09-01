using Authorization.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
                Create("Jwt:SetPasswordAudience", "Authorization"),
                Create("Jwt:PrivateKeyPem", null),
                Create("Jwt:PublicKeyPem", null),
                Create("UserPasswords:KeepUserPasswordsCount", 5),
                Create("UserPasswords:MaxAccessFailedCount", 6),
                Create("UserPasswords:LockoutPeriod", "00:30:00"),
                Create("UserPasswords:PasswordExpirationPeriod", null),
                Create("Authorization:AuthorizeUnconfirmedEmailPeriod", "00:00:00"),
                Create("Emails:Username", null),
                Create("Emails:Password", null),
                Create("Emails:From", "info@benraz.com"),
                Create("Emails:FromDisplayName", "Benraz"),
                Create("Emails:CompanyName", "Benraz"),
                Create("Emails:CompanyLogoUrl", "https://www.benraz.com/wp-content/themes/benraz-by-dorzki/assets/images/logo.svg"),
                Create("Emails:CompanyEmail", "support@benraz.com"),
                Create("Emails:CompanyPhone", "1-888-497-5499"),
                Create("Emails:ConfirmEmailSubject", "Email confirmation", "Confirmation email subject"),
                Create("Emails:ConfirmEmailTemplateId", "8cd5b7d0-df9e-4b66-93ff-1f7e0983e1e4", "Confirmation email template identifier"),
                Create("Emails:ResetPasswordSubject", "Reset password"),
                Create("Emails:ResetPasswordTemplateId", "366ea284-d131-47d4-9b27-4c28f71dea8a"));
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


