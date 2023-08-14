namespace Authorization.Domain.Authorization
{
    /// <summary>
    /// Authorization claims.
    /// </summary>
    public static class AuthorizationClaims
    {
        /// <summary>
        /// Read settings claim.
        /// </summary>
        public const string SETTINGS_READ = "Authorization-settings-read";

        /// <summary>
        /// Add settings claim.
        /// </summary>
        public const string SETTINGS_ADD = "Authorization-settings-add";

        /// <summary>
        /// Update settings claim.
        /// </summary>
        public const string SETTINGS_UPDATE = "Authorization-settings-update";

        /// <summary>
        /// Delete settings claim.
        /// </summary>
        public const string SETTINGS_DELETE = "Authorization-settings-delete";

        /// <summary>
        /// Execute job claim.
        /// </summary>
        public const string JOB_EXECUTE = "Authorization-job-execute";
    }
}

