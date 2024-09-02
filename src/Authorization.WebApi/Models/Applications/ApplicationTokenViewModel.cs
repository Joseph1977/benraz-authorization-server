using System;

namespace Authorization.WebApi.Models.Applications
{
    /// <summary>
    /// Application token view model.
    /// </summary>
    public class ApplicationTokenViewModel : ApplicationTokenViewModelBase
    {
        /// <summary>
        /// Token identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Create time in UTC.
        /// </summary>
        public DateTime CreateTimeUtc { get; set; }

        /// <summary>
        /// Created by.
        /// </summary>
        public string? CreatedBy { get; set; }
    }
}


