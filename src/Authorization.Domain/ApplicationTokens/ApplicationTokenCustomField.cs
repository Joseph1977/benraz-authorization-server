using Benraz.Infrastructure.Common.EntityBase;
using System;

namespace Authorization.Domain.ApplicationTokens
{
    /// <summary>
    /// Application token custom field.
    /// </summary>
    public class ApplicationTokenCustomField : IEntity<Guid>
    {
        /// <summary>
        /// Field identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Application token identifier.
        /// </summary>
        public Guid ApplicationTokenId { get; set; }

        /// <summary>
        /// Key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        public string Value { get; set; }
    }
}


