using Benraz.Infrastructure.Common.EntityBase;

namespace Authorization.Domain.Applications
{
    /// <summary>
    /// Application URL type.
    /// </summary>
    public class ApplicationUrlType : IEntity<ApplicationUrlTypeCode>
    {
        /// <summary>
        /// Application URL type code.
        /// </summary>
        public ApplicationUrlTypeCode Id { get; set; }

        /// <summary>
        /// Type name.
        /// </summary>
        public string Name { get; set; }
    }
}


