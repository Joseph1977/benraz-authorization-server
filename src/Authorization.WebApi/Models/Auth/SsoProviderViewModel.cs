using Benraz.Infrastructure.Domain.Authorization;

namespace Authorization.WebApi.Models.Auth
{
    /// <summary>
    /// SSO provider view model.
    /// </summary>
    public class SsoProviderViewModel
    {
        /// <summary>
        /// Code.
        /// </summary>
        public SsoProviderCode Code { get; set; }
    }
}


