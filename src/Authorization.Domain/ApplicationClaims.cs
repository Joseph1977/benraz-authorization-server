namespace Authorization.Domain
{
    /// <summary>
    /// Authorization application claims.
    /// </summary>
    public static class ApplicationClaims
    {
        /// <summary>
        /// Read application claim.
        /// </summary>
        public const string APPLICATION_READ = "authorization-application-read";

        /// <summary>
        /// Add application claim.
        /// </summary>
        public const string APPLICATION_ADD = "authorization-application-add";

        /// <summary>
        /// Update application claim.
        /// </summary>
        public const string APPLICATION_UPDATE = "authorization-application-update";

        /// <summary>
        /// Delete application claim.
        /// </summary>
        public const string APPLICATION_DELETE = "authorization-application-delete";

        /// <summary>
        /// Read users claim.
        /// </summary>
        public const string USER_READ = "authorization-user-read";

        /// <summary>
        /// Read one user claim.
        /// </summary>
        public const string USER_READ_ONE = "authorization-user-read-one";

        /// <summary>
        /// Add user claim.
        /// </summary>
        public const string USER_ADD = "authorization-user-add";

        /// <summary>
        /// Update user claim.
        /// </summary>
        public const string USER_UPDATE = "authorization-user-update";

        /// <summary>
        /// Delete user claim.
        /// </summary>
        public const string USER_DELETE = "authorization-user-delete";

        /// <summary>
        /// Read user status claim.
        /// </summary>
        public const string USER_STATUS_READ = "authorization-user-status-read";

        /// <summary>
        /// Update user payment service suspended status claim.
        /// </summary>
        public const string USER_STATUS_SUSPEND = "authorization-user-status-suspend";

        /// <summary>
        /// Update user block status claim.
        /// </summary>
        public const string USER_STATUS_BLOCK = "authorization-user-status-block";

        /// <summary>
        /// Unlock user claim.
        /// </summary>
        public const string USER_UNLOCK = "authorization-user-unlock";

        /// <summary>
        /// Read user roles claim.
        /// </summary>
        public const string USER_ROLE_READ = "authorization-user-role-read";

        /// <summary>
        /// Update user roles claim.
        /// </summary>
        public const string USER_ROLE_UPDATE = "authorization-user-role-update";

        /// <summary>
        /// Read user claims claim.
        /// </summary>
        public const string USER_CLAIM_READ = "authorization-user-claim-read";

        /// <summary>
        /// Update user claims claim.
        /// </summary>
        public const string USER_CLAIM_UPDATE = "authorization-user-claim-update";

        /// <summary>
        /// Read user email claim.
        /// </summary>
        public const string USER_EMAIL_READ = "authorization-user-email-read";

        /// <summary>
        /// Update user email claim.
        /// </summary>
        public const string USER_EMAIL_UPDATE = "authorization-user-email-update";

        /// <summary>
        /// Verify user email claim.
        /// </summary>
        public const string USER_EMAIL_VERIFY = "authorization-user-email-verify";

        /// <summary>
        /// Read user phone claim.
        /// </summary>
        public const string USER_PHONE_READ = "authorization-user-phone-read";

        /// <summary>
        /// Verify user phone claim.
        /// </summary>
        public const string USER_PHONE_VERIFY = "authorization-user-phone-verify";

        /// <summary>
        /// Update user password claim.
        /// </summary>
        public const string USER_PASSWORD_UPDATE = "authorization-user-password-update";

        /// <summary>
        /// Reset user password claim.
        /// </summary>
        public const string USER_PASSWORD_RESET = "authorization-user-password-reset";

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
        /// Change profile password claim.
        /// </summary>
        public const string PROFILE_PASSWORD_CHANGE = "authorization-profile-password-change";

        /// <summary>
        /// Set new profile password without providing the old password claim.
        /// </summary>
        public const string PROFILE_PASSWORD_SET = "authorization-profile-password-set";

        /// <summary>
        /// Exchange token claim.
        /// </summary>
        public const string TOKEN_EXCHANGE = "authorization-token-exchange";
    }
}


