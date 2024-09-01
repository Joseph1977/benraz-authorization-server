using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Authorization.Infrastructure.Gateways.MicrosoftGraph.Messages
{
    /// <summary>
    /// Profile response.
    /// </summary>
    public class GetProfileResponse : MicrosoftGraphResponseBase
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Display name.
        /// </summary>
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Given name.
        /// </summary>
        [JsonPropertyName("givenName")]
        public string GivenName { get; set; }

        /// <summary>
        /// Surname.
        /// </summary>
        [JsonPropertyName("surname")]
        public string Surname { get; set; }

        /// <summary>
        /// User principal name.
        /// </summary>
        [JsonPropertyName("userPrincipalName")]
        public string UserPrincipalName { get; set; }

        /// <summary>
        /// Job title.
        /// </summary>
        [JsonPropertyName("jobTitle")]
        public string JobTitle { get; set; }

        /// <summary>
        /// Mail.
        /// </summary>
        [JsonPropertyName("mail")]
        public string Mail { get; set; }

        /// <summary>
        /// Mobile phone.
        /// </summary>
        [JsonPropertyName("mobilePhone")]
        public string MobilePhone { get; set; }

        /// <summary>
        /// Business phone.
        /// </summary>
        [JsonPropertyName("businessPhones")]
        public ICollection<string> BusinessPhones { get; set; }

        /// <summary>
        /// Office location.
        /// </summary>
        [JsonPropertyName("officeLocation")]
        public string OfficeLocation { get; set; }

        /// <summary>
        /// Preferred language.
        /// </summary>
        [JsonPropertyName("preferredLanguage")]
        public string PreferredLanguage { get; set; }
    }
}


