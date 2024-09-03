namespace Authorization.WebApi.Models.Roles
{
    /// <summary>
    /// Role view model.
    /// </summary>
    public class RoleViewModel
    {
        /// <summary>
        /// Role identifier.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Role name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Concurrency stamp.
        /// </summary>
        public string? ConcurrencyStamp { get; set; }
    }
}


