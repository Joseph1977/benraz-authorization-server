using Benraz.Infrastructure.Common.EntityBase;
using System;

namespace Authorization.Domain.Applications
{
    /// <summary>
    /// Application URL.
    /// </summary>
    public class ApplicationUrl : IEntity<Guid>
    {
        /// <summary>
        /// Application URL identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Application identifier.
        /// </summary>
        public Guid ApplicationId { get; set; }

        /// <summary>
        /// Application URL type code.
        /// </summary>
        public ApplicationUrlTypeCode TypeCode { get; set; }

        /// <summary>
        /// URL value.
        /// </summary>
        public string Url { get; set; }
    }
}


