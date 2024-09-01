namespace Authorization.WebApi.Models.Auth
{
    /// <summary>
    /// Token response view model.
    /// </summary>
    public class TokenResponseViewModel
    {
        /// <summary>
        /// Access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Error.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Creates view model with access token.
        /// </summary>
        /// <param name="accessToken">Access token.</param>
        /// <returns>Token view model.</returns>
        public static TokenResponseViewModel CreateSuccess(string accessToken)
        {
            return new TokenResponseViewModel
            {
                AccessToken = accessToken
            };
        }

        /// <summary>
        /// Creates view model with error.
        /// </summary>
        /// <param name="error">Error.</param>
        /// <returns>Token view model.</returns>
        public static TokenResponseViewModel CreateError(string error)
        {
            return new TokenResponseViewModel
            {
                Error = error
            };
        }
    }
}


