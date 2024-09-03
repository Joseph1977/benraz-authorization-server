using Authorization.Domain.Emails;
using Microsoft.AspNetCore.Identity;
using Benraz.Infrastructure.Domain.Common;
using System.Threading;
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

        /// <summary>
        /// Get user by email.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <returns>User.</returns>
        Task<User> GetUserByEmailAsync(string email, bool? includeEmployee = false);

        /// <summary>
        /// Is verification code valid.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="verificationCode">Verification code.</param>
        /// <param name="mfaCode">Mfa code.</param>
        /// <returns>Is verification code valid.</returns>
        Task<bool> IsVerificationCodeValidAsync(string userId, string verificationCode, MfaCode mfaCode);

        /// <summary>
        /// Get action URL.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="actionType">Action type.</param>
        /// <returns>Action URL.</returns>
        Task<string> GetActionUrlParametersAsync(string userId, EmailActionType actionType);

        /// <summary>
        /// Create mfa.
        /// </summary>
        /// <param name="user">User model.</param>
        /// <param name="viewModel">Create mfa view model.</param>
        /// <returns>Create mfa view result.</returns>
        Task<CreateMfaViewResult> CreateMfaAsync(User user, CreateMfaViewModel viewModel);

        /// <summary>
        /// Get one time token by mfa code.
        /// </summary>
        /// <param name="user">User model.</param>
        /// <param name="viewModel">Get token by mfa code model.</param>
        /// <returns>Get token by mfa code result.</returns>
        Task<GetTokenByMfaCodeResult> GetOneTimeTokenByMfaCodeAsync(User user, GetTokenByMfaCodeModel viewModel);

        /// <summary>
        /// Update user.
        /// </summary>
        /// <param name="user">User model.</param>
        /// <param name="cancellationToken">Cancellation token type.</param>
        /// <returns>Identity result.</returns>
        Task<IdentityResult> UpdateUserAsync(User user, CancellationToken cancellationToken);
    }
}


