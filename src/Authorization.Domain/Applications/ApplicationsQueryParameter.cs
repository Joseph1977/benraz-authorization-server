namespace Authorization.Domain.Applications
{
    /// <summary>
    /// Applications query parameter.
    /// </summary>
    public enum ApplicationsQueryParameter
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        Id = 1,

        /// <summary>
        /// Name.
        /// </summary>
        Name = 2,

        /// <summary>
        /// Create time in UTC.
        /// </summary>
        CreateTimeUtc = 3,

        /// <summary>
        /// Last update time in UTC.
        /// </summary>
        UpdateTimeUtc = 4,

        /// <summary>
        /// Name of a user who created the application.
        /// </summary>
        CreatedBy = 5,

        /// <summary>
        /// Name of a user who made last changes.
        /// </summary>
        UpdatedBy = 6,

        /// <summary>
        /// Audience.
        /// </summary>
        Audience = 7
    }
}


