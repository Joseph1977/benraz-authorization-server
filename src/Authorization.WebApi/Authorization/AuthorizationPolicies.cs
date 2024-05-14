namespace Authorization.WebApi.Authorization
{
    /// <summary>
    /// Authorization policies.
    /// </summary>
    public static class AuthorizationPolicies
    {
        /// <summary>
        /// Read settings policy.
        /// </summary>
        public const string SETTINGS_READ = "settings-read";

        /// <summary>
        /// Add settings policy.
        /// </summary>
        public const string SETTINGS_ADD = "settings-add";

        /// <summary>
        /// Update settings policy.
        /// </summary>
        public const string SETTINGS_UPDATE = "settings-update";

        /// <summary>
        /// Delete settings policy.
        /// </summary>
        public const string SETTINGS_DELETE = "settings-delete";

        /// <summary>
        /// Execute job policy.
        /// </summary>
        public const string JOB_EXECUTE = "job-execute";

        /// <summary>
        /// Read claims policy.
        /// </summary>
        public const string CLAIM_READ = "claim-read";

        /// <summary>
        /// Add claim policy.
        /// </summary>
        public const string CLAIM_ADD = "claim-add";

        /// <summary>
        /// Delete claim policy.
        /// </summary>
        public const string CLAIM_DELETE = "claim-delete";

        /// <summary>
        /// Read roles policy.
        /// </summary>
        public const string ROLE_READ = "role-read";

        /// <summary>
        /// Add role policy.
        /// </summary>
        public const string ROLE_ADD = "role-add";

        /// <summary>
        /// Update role policy.
        /// </summary>
        public const string ROLE_UPDATE = "role-update";

        /// <summary>
        /// Delete role policy.
        /// </summary>
        public const string ROLE_DELETE = "role-delete";
    }
}

