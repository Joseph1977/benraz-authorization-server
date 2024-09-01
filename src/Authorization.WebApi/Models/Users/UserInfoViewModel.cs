using System;
using Benraz.Infrastructure.Domain.Authorization;

namespace Authorization.WebApi.Models.Users
{
    /// <summary>
    /// User info view model.
    /// </summary>
    public class UserInfoViewModel
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Full name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Is email confirmed.
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// Phone number.
        /// </summary>
        public string PhoneNumber { get; set; }
        
        /// <summary>
        /// Is phone number confirmed.
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Status code.
        /// </summary>
        public UserStatusCode StatusCode { get; set; }

        /// <summary>
        /// Is SSO logins only.
        /// </summary>
        public bool IsSsoOnly { get; set; }

        /// <summary>
        /// Count of failed access attempts.
        /// </summary>
        public int AccessFailedCount { get; set; }

        /// <summary>
        /// Possibility to be lockout.
        /// </summary>
        public bool LockoutEnabled { get; set; }

        /// <summary>
        /// Date when lockout ends.
        /// </summary>
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}


