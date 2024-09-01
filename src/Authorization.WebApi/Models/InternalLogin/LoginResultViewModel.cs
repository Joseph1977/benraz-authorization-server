using Authorization.Domain;

namespace Authorization.WebApi.Models.InternalLogin
{
    /// <summary>
    /// Login result view model.
    /// </summary>
    public class LoginResultViewModel
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Error.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Error reason code.
        /// </summary>
        public AuthorizationFailedReasonCode? ErrorReasonCode { get; set; }

        /// <summary>
        /// Callback URL.
        /// </summary>
        public string CallbackUrl { get; set; }
    }
}


