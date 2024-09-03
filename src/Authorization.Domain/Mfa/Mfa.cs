using Benraz.Infrastructure.Common.EntityBase;
using Benraz.Infrastructure.Domain.Common;

namespace Authorization.Domain.Mfa
{
    /// <summary>
    /// Mfa.
    /// </summary>
    public class Mfa : IEntity<MfaCode>
    {
        /// <summary>
        /// Mfa code identifier.
        /// </summary>
        public MfaCode Id { get; set; }

        /// <summary>
        /// Mfa code name.
        /// </summary>
        public string Name { get; set; }
    }
}
