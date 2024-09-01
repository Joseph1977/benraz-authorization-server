namespace Authorization.WebApi.Authorization
{
    /// <summary>
    /// Authorization application policies.
    /// </summary>
    public static class ApplicationPolicies
    {
        /// <summary>
        /// Read application policy.
        /// </summary>
        public const string APPLICATION_READ = "application-read";

        /// <summary>
        /// Add application policy. 
        /// </summary>
        public const string APPLICATION_ADD = "application-add";

        /// <summary>
        /// Update application policy.
        /// </summary>
        public const string APPLICATION_UPDATE = "application-update";

        /// <summary>
        /// Delete application policy.
        /// </summary>
        public const string APPLICATION_DELETE = "application-delete";

        /// <summary>
        /// Read users policy.
        /// </summary>
        public const string USER_READ = "user-read";

        /// <summary>
        /// Read one user policy.
        /// </summary>
        public const string USER_READ_ONE = "user-read-one";

        /// <summary>
        /// Add user policy.
        /// </summary>
        public const string USER_ADD = "user-add";

        /// <summary>
        /// Update user policy.
        /// </summary>
        public const string USER_UPDATE = "user-update";

        /// <summary>
        /// Delete user policy.
        /// </summary>
        public const string USER_DELETE = "user-delete";

        /// <summary>
        /// Read user status policy.
        /// </summary>
        public const string USER_STATUS_READ = "user-status-read";

        /// <summary>
        /// Update user payment service suspended status policy.
        /// </summary>
        public const string USER_STATUS_SUSPEND = "user-status-suspend";

        /// <summary>
        /// Unlock user policy.
        /// </summary>
        public const string USER_STATUS_BLOCK = "user-status-block";

        /// <summary>
        /// Update user unlock policy.
        /// </summary>
        public const string USER_UNLOCK = "user-unlock";

        /// <summary>
        /// Read user roles policy.
        /// </summary>
        public const string USER_ROLE_READ = "user-role-read";

        /// <summary>
        /// Update user roles policy.
        /// </summary>
        public const string USER_ROLE_UPDATE = "user-role-update";

        /// <summary>
        /// Read user claims policy.
        /// </summary>
        public const string USER_CLAIM_READ = "user-claim-read";

        /// <summary>
        /// Update user claims policy.
        /// </summary>
        public const string USER_CLAIM_UPDATE = "user-claim-update";

        /// <summary>
        /// Read user email policy.
        /// </summary>
        public const string USER_EMAIL_READ = "user-email-read";

        /// <summary>
        /// Update user email policy.
        /// </summary>
        public const string USER_EMAIL_UPDATE = "user-email-update";

        /// <summary>
        /// Verify user email policy.
        /// </summary>
        public const string USER_EMAIL_VERIFY = "user-email-verify";

        /// <summary>
        /// Read user phone number policy.
        /// </summary>
        public const string USER_PHONE_READ = "user-phone-read";

        /// <summary>
        /// Verify user phone policy.
        /// </summary>
        public const string USER_PHONE_VERIFY = "user-phone-verify";

        /// <summary>
        /// Update user password policy.
        /// </summary>
        public const string USER_PASSWORD_UPDATE = "user-password-update";

        /// <summary>
        /// Reset user password policy.
        /// </summary>
        public const string USER_PASSWORD_RESET = "user-password-reset";

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
        /// Change password policy.
        /// </summary>
        public const string INTERNAL_LOGIN_CHANGE_PASSWORD = "internal-login-change-password";

        /// <summary>
        /// Set new password without providing the old password policy.
        /// </summary>
        public const string INTERNAL_LOGIN_SET_PASSWORD = "internal-login-set-password";

        /// <summary>
        /// Exchange token policy.
        /// </summary>
        public const string TOKEN_EXCHANGE = "token-exchange";
    }
}


