using Benraz.Infrastructure.Domain.Authorization;
using System;

namespace Authorization.WebApi.Models.Auth
{
    /// <summary>
    /// Issue token request view model.
    /// </summary>
    public class TokenRequestViewModel
    {
        /// <summary>
        /// Application identifier.
        /// </summary>
        public Guid ApplicationId { get; set; }

        /// <summary>
        /// SSO provider code.
        /// </summary>
        public SsoProviderCode SsoProviderCode { get; set; }

        /// <summary>
        /// User name.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Grant type.
        /// </summary>
        public string GrantType { get; set; }
    }
}


