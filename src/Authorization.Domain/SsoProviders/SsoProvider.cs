using Benraz.Infrastructure.Common.EntityBase;
using Benraz.Infrastructure.Domain.Authorization;

namespace Authorization.Domain.SsoProviders
{
    /// <summary>
    /// SSO provider.
    /// </summary>
    public class SsoProvider : IEntity<SsoProviderCode>
    {
        /// <summary>
        /// SSO provider identifier.
        /// </summary>
        public SsoProviderCode Id { get; set; }

        /// <summary>
        /// SSO provider name.
        /// </summary>
        public string Name { get; set; }
    }
}


