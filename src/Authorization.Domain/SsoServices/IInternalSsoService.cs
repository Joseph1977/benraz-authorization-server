namespace Authorization.Domain.SsoServices
{
    /// <summary>
    /// Internal SSO service.
    /// </summary>
    public interface IInternalSsoService : ISsoService
    {
        /// <summary>
        /// Creates confirm email URL.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <param name="confirmEmailToken">Confirm email token.</param>
        /// <returns>Confirm email URL.</returns>
        string CreateConfirmEmailUrl(string username, string confirmEmailToken);

        /// <summary>
        /// Creates set password URL.
        /// </summary>
        /// <param name="resetPasswordToken">Reset password token.</param>
        /// <param name="accessToken">Access token to set password.</param>
        /// <returns>Set password URL.</returns>
        string CreateSetPasswordUrl(string resetPasswordToken, string accessToken);
    }
}


