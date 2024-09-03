namespace Authorization.Domain
{
    /// <summary>
    /// Authorization failed reason code.
    /// </summary>
    public enum AuthorizationFailedReasonCode
    {
        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown = 1,

        /// <summary>
        /// Invalid credentials.
        /// </summary>
        InvalidCredentials = 2,

        /// <summary>
        /// User is locked out because of too many login attempts.
        /// </summary>
        Locked = 3,

        /// <summary>
        /// User status is suspended or blocked.
        /// </summary>
        Suspended = 4,

        /// <summary>
        /// User email is unconfirmed.
        /// </summary>
        EmailUnconfirmed = 5
    }
}


