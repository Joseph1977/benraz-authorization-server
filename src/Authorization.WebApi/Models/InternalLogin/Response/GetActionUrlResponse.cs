namespace Authorization.WebApi.Models.InternalLogin.Response
{
    /// <summary>
    /// Get action url response.
    /// </summary>
    public class GetActionUrlResponse
    {
        /// <summary>
        /// User full name.
        /// </summary>
        public string? UserFullname { get; set; }

        /// <summary>
        /// Action url.
        /// </summary>
        public string? ActionUrl { get; set; }
    }
}
