using System;
using System.Collections.Generic;

namespace Authorization.WebApi.Models.Applications
{
    /// <summary>
    /// Application view model.
    /// </summary>
    public class ApplicationViewModel
    {
        /// <summary>
        /// Application identifier.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Application name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Application audience.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Is access token attachment as a cookie enabled.
        /// </summary>
        public bool IsAccessTokenCookieEnabled { get; set; }

        /// <summary>
        /// Access token cookie name.
        /// </summary>
        public string AccessTokenCookieName { get; set; }

        /// <summary>
        /// Is access token attachment as a URL fragment disabled.
        /// </summary>
        public bool IsAccessTokenFragmentDisabled { get; set; }

        /// <summary>
        /// Access token validity period.
        /// </summary>
        public string AccessTokenValidityPeriod { get; set; }

        /// <summary>
        /// Name of a user who created the application.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Create time in UTC.
        /// </summary>
        public DateTime CreateTimeUtc { get; set; }

        /// <summary>
        /// Name of a user who made last changes.
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Update time in UTC.
        /// </summary>
        public DateTime UpdateTimeUtc { get; set; }

        /// <summary>
        /// Application SSO providers.
        /// </summary>
        public ICollection<ApplicationSsoConnectionViewModel> SsoConnections { get; set; }

        /// <summary>
        /// Application URLs.
        /// </summary>
        public ICollection<ApplicationUrlViewModel> Urls { get; set; }
    }
}


