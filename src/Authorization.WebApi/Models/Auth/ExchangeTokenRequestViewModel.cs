using System;

namespace Authorization.WebApi.Models.Auth
{
    /// <summary>
    /// Exchange token request view model.
    /// </summary>
    public class ExchangeTokenRequestViewModel
    {
        /// <summary>
        /// Application identifier.
        /// </summary>
        public Guid ApplicationId { get; set; }

        /// <summary>
        /// Access token to exchange.
        /// </summary>
        public string? AccessToken { get; set; }
    }
}


