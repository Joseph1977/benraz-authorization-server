using Authorization.Domain.SsoConnections;
using Benraz.Infrastructure.Common.EntityBase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Authorization.Domain.Applications
{
    /// <summary>
    /// Application.
    /// </summary>
    public class Application : AggregateRootBase<Guid>
    {
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
        public TimeSpan? AccessTokenValidityPeriod { get; set; }

        /// <summary>
        /// Name of a user who created the application.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Name of a user who made last changes.
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Application URLs.
        /// </summary>
        public ICollection<ApplicationUrl> Urls { get; set; }

        /// <summary>
        /// Application SSO connections.
        /// </summary>
        public ICollection<SsoConnection> SsoConnections { get; set; }

        /// <summary>
        /// Creates entity.
        /// </summary>
        public Application()
        {
            Urls = new List<ApplicationUrl>();
            SsoConnections = new List<SsoConnection>();
        }

        /// <summary>
        /// Returns URL for authorization callbacks.
        /// </summary>
        /// <returns>Returns URL for authorization callbacks.</returns>
        public string GetCallbackUrl()
        {
            return GetApplicationUrl(ApplicationUrlTypeCode.Callback)?.Url;
        }

        /// <summary>
        /// Returns enabled SSO connections.
        /// </summary>
        /// <returns>Enabled SSO connections.</returns>
        public ICollection<SsoConnection> GetEnabledSsoConnections()
        {
            return SsoConnections
                .Where(x => x.IsEnabled)
                .ToList();
        }

        private ApplicationUrl GetApplicationUrl(ApplicationUrlTypeCode typeCode)
        {
            return Urls.FirstOrDefault(x => x.TypeCode == typeCode);
        }
    }
}


