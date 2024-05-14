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

        /// <summary>
        /// Read claims claim.
        /// </summary>
        public const string CLAIM_READ = "authorization-claim-read";

        /// <summary>
        /// Add claim claim.
        /// </summary>
        public const string CLAIM_ADD = "authorization-claim-add";

        /// <summary>
        /// Delete claim claim.
        /// </summary>
        public const string CLAIM_DELETE = "authorization-claim-delete";

        /// <summary>
        /// Read employee claim.
        /// </summary>
        public const string EMPLOYEE_READ = "authorization-employee-read";

        /// <summary>
        /// Read roles claim.
        /// </summary>
        public const string ROLE_READ = "authorization-role-read";

        /// <summary>
        /// Add role claim.
        /// </summary>
        public const string ROLE_ADD = "authorization-role-add";

        /// <summary>
        /// Update role claim.
        /// </summary>
        public const string ROLE_UPDATE = "authorization-role-update";

        /// <summary>
        /// Delete role claim.
        /// </summary>
        public const string ROLE_DELETE = "authorization-role-delete";
    }
}

