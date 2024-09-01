using System.Threading.Tasks;

namespace Authorization.Domain.Users
{
    /// <summary>
    /// User passwords service.
    /// </summary>
    public interface IUserPasswordsService
    {
        /// <summary>
        /// Checks new password.
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="newPassword">New password.</param>
        /// <returns>Check result.</returns>
        Task<bool> CheckPasswordAsync(User user, string newPassword);

        /// <summary>
        /// Checks if user password expired.
        /// </summary>
        /// <param name="user">User.</param>
        /// <returns>If password expired.</returns>
        Task<bool> IsPasswordExpiredAsync(User user);

        /// <summary>
        /// Saves new password.
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="userPassword">User password.</param>
        /// <returns>Task.</returns>
        Task SaveNewPasswordAsync(User user, UserPassword userPassword);

        /// <summary>
        /// Processes user access failed.
        /// </summary>
        /// <param name="user">User.</param>
        /// <returns>Task.</returns>
        Task ProcessAccessFailedAsync(User user);
    }
}


