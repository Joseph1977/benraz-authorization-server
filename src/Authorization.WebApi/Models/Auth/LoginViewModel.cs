using Benraz.Infrastructure.Domain.Authorization;
using System;

namespace Authorization.WebApi.Models.Auth
{
    /// <summary>
    /// Login view model.
    /// </summary>
    public class LoginViewModel
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
        /// URL to be included into final redirect as a parameter.
        /// </summary>
        public string? ReturnUrl { get; set; }
    }
}


