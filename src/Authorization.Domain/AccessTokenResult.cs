namespace Authorization.Domain
{
    /// <summary>
    /// Access token result.
    /// </summary>
    public class AccessTokenResult
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
        /// Is password expired access token.
        /// </summary>
        public bool IsPasswordExpired { get; set; }

        /// <summary>
        /// Creates result.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="accessToken">Access token.</param>
        /// <param name="isPasswordExpired">Is password expired.</param>
        public AccessTokenResult(string userId, string accessToken, bool isPasswordExpired = false)
        {
            UserId = userId;
            AccessToken = accessToken;
            IsPasswordExpired = isPasswordExpired;
        }
    }
}


