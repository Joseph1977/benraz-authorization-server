namespace Authorization.WebApi.Models.Applications
{
    /// <summary>
    /// Application token claim view model.
    /// </summary>
    public class ApplicationTokenClaimViewModel
    {
        /// <summary>
        /// Claim type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Claim value.
        /// </summary>
        public string Value { get; set; }
    }
}


