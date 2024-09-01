using System;
using System.Collections.Generic;

namespace Authorization.WebApi.Models.Applications
{
    /// <summary>
    /// Application token base view model.
    /// </summary>
    public abstract class ApplicationTokenViewModelBase
    {
        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Expiration time in UTC.
        /// </summary>
        public DateTime? ExpirationTimeUtc { get; set; }

        /// <summary>
        /// Roles.
        /// </summary>
        public ICollection<string> Roles { get; set; }

        /// <summary>
        /// Claims.
        /// </summary>
        public ICollection<ApplicationTokenClaimViewModel> Claims { get; set; }

        /// <summary>
        /// Custom fields.
        /// </summary>
        public ICollection<ApplicationTokenCustomFieldViewModel> CustomFields { get; set; }
    }
}


