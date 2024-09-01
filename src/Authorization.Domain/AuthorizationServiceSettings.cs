using System;

namespace Authorization.Domain
{
    /// <summary>
    /// Authorization service settings.
    /// </summary>
    public class AuthorizationServiceSettings
    {
        /// <summary>
        /// Period user can be authorized without email confirmation.
        /// </summary>
        public TimeSpan AuthorizeUnconfirmedEmailPeriod { get; set; }
    }
}


