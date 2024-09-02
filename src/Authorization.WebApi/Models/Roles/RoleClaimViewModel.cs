namespace Authorization.WebApi.Models.Roles
{
    /// <summary>
    /// Role claim view model.
    /// </summary>
    public class RoleClaimViewModel
    {
        /// <summary>
        /// Claim type.
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Claim value.
        /// </summary>
        public string? Value { get; set; }
    }
}


