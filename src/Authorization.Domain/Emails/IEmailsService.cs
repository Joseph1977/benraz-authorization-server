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
        /// <param name="model">Email address confirmation email.</param>
        /// <returns>Task.</returns>
        Task SendConfirmationEmailAsync(ConfirmationEmailModel model);

        /// <summary>
        /// Sends reset password email.
        /// </summary>
        /// <param name="model">Reset password email model.</param>
        /// <returns>Task.</returns>
        Task SendResetPasswordEmailAsync(ResetPasswordEmailModel model);
    }
}


