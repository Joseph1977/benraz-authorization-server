using System.Threading.Tasks;

namespace Authorization.Domain.Users
{
    /// <summary>
    /// Users service.
    /// </summary>
    public interface IUsersService
    {
        /// <summary>
        /// Registers new user.
        /// </summary>
        /// <param name="model">Register user model.</param>
        /// <returns>Result.</returns>
        Task<RegisterUserResult> RegisterAsync(RegisterUserModel model);

        /// <summary>
        /// Sends email address confirmation email.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>Task.</returns>
        Task SendConfirmationEmailAsync(string userId);

        /// <summary>
        /// Confirms user email.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="token">Email confirmation token.</param>
        /// <returns>Task.</returns>
        Task ConfirmEmailAsync(string userId, string token);

        /// <summary>
        /// Resets user password.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>Task.</returns>
        Task ResetPasswordAsync(string userId);
    }
}


