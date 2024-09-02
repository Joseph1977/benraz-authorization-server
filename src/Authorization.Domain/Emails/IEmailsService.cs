using Authorization.Domain.Emails.Confirmation;
using Authorization.Domain.Emails.MfaCode;
using Authorization.Domain.Emails.ResetPassword;
using Authorization.Domain.Users;
using System.Threading.Tasks;

namespace Authorization.Domain.Emails
{
    /// <summary>
    /// Emails service.
    /// </summary>
    public interface IEmailsService
    {
        /// <summary>
        /// Sends email address confirmation email.
        /// </summary>
        /// <param name="parameters">Email address confirmation parameters.</param>
        /// <returns>Task.</returns>
        Task SendConfirmationEmailAsync(ConfirmationEmailParameters parameters);

        /// <summary>
        /// Sends reset password email.
        /// </summary>
        /// <param name="parameters">Reset password email parameters.</param>
        /// <returns>Task.</returns>
        Task SendResetPasswordEmailAsync(ResetPasswordEmailParameters parameters);

        /// <summary>
        /// Sends mfa code email.
        /// </summary>
        /// <param name="parameters">Mfa code email parameters.</param>
        /// <returns>Task.</returns>
        Task SendMfaCodeEmailAsync(MfaCodeEmailParameters parameters);

        /// <summary>
        /// Sends user login email.
        /// </summary>
        /// <param name="viewModel">User model.</param>
        /// <returns>Task.</returns>
        Task SendUserLoginEmailAsync(User viewModel);
    }
}