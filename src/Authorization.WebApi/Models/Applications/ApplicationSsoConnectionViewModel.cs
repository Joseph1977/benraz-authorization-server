using Benraz.Infrastructure.Domain.Authorization;

namespace Authorization.WebApi.Models.Applications
{
    /// <summary>
    /// Application SSO connection view model.
    /// </summary>
    public class ApplicationSsoConnectionViewModel
    {
        /// <summary>
        /// SSO provider code.
        /// </summary>
        public SsoProviderCode SsoProviderCode { get; set; }

        /// <summary>
        /// SSO authorization URL.
        /// </summary>
        public string? AuthorizationUrl { get; set; }

        /// <summary>
        /// SSO token URL.
        /// </summary>
        public string? TokenUrl { get; set; }

        /// <summary>
        /// SSO client identifier.
        /// </summary>
        public string? ClientId { get; set; }

        /// <summary>
        /// SSO client secret.
        /// </summary>
        public string? ClientSecret { get; set; }

        /// <summary>
        /// New SSO client secret to update old one.
        /// </summary>
        public string? NewClientSecret { get; set; }

        /// <summary>
        /// SSO scope.
        /// </summary>
        public string? Scope { get; set; }

        /// <summary>
        /// Is SSO connection enabled.
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}


