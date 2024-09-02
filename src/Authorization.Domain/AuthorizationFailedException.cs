using System;

namespace Authorization.Domain
{
    /// <summary>
    /// Authorization failed exception.
    /// </summary>
    public class AuthorizationFailedException : InvalidOperationException
    {
        /// <summary>
        /// User identifier if applicable.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Reason.
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Reason code.
        /// </summary>
        public AuthorizationFailedReasonCode ReasonCode { get; set; }

        /// <summary>
        /// Mfa required.
        /// </summary>
        public bool MfaRequired { get; set; }

        /// <summary>
        /// Creates exception.
        /// </summary>
        public AuthorizationFailedException()
            : this(null, null)
        {
        }

        /// <summary>
        /// Creates exception.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="reason">Reason.</param>
        public AuthorizationFailedException(string message, string reason)
            : base(message)
        {
            Reason = reason;
        }
    }
}
