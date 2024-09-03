using Authorization.Domain.Emails.Confirmation;
using Authorization.Domain.Emails.MfaCode;
using Authorization.Domain.Emails.ResetPassword;
using Authorization.Domain.Emails.UserLogin;
using System.Threading.Tasks;

namespace Authorization.Domain.Emails
{
    /// <summary>
    /// Emails notification factory.
    /// </summary>
    public interface IEmailsNotificationFactory
    {
        /// <summary>
        /// Creates new confirmation notification.
        /// </summary>
        /// <param name="model">New confirmation email notification model.</param>
        /// <returns>Email body.</returns>
        Task<string> CreateNewConfirmationNotificationAsync(ConfirmationEmailModel model);

        /// <summary>
        /// Creates new reset password notification.
        /// </summary>
        /// <param name="model">New reset password email notification model.</param>
        /// <returns>Email body.</returns>
        Task<string> CreateNewResetPasswordNotificationAsync(ResetPasswordEmailModel model);

        /// <summary>
        /// Creates new mfa code notification.
        /// </summary>
        /// <param name="model">Mfa code email notification model.</param>
        /// <returns>Email body.</returns>
        Task<string> CreateMfaCodeNotificationAsync(MfaCodeEmailModel model);

        /// <summary>
        /// Creates new user login notification.
        /// </summary>
        /// <param name="model">User login email model.</param>
        /// <returns>Email body.</returns>
        Task<string> CreateNewUserLoginNotificationAsync(UserLoginEmailModel model);
    }
}