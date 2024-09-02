namespace Authorization.Domain.Users
{
    /// <summary>
    ///  Get token after verify mfa code result.
    /// </summary>
    public class GetTokenByMfaCodeResult
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
        /// Code.
        /// </summary>
        public string Code { get; set; }
    }
}
