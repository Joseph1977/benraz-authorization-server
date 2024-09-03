using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Domain.Users
{
    /// <summary>
    /// User passwords service.
    /// </summary>
    public class UserPasswordsService : IUserPasswordsService
    {
        private readonly IUserPasswordsRepository _userPasswordsRepository;
        private readonly UserManager<User> _userManager;
        private readonly UserPasswordsServiceSettings _settings;

        /// <summary>
        /// Creates user passwords service.
        /// </summary>
        /// <param name="userPasswordsRepository">User passwords repository.</param>
        /// <param name="userManager">User manager.</param>
        /// <param name="settings">User passwords service settings.</param>
        public UserPasswordsService(
            IUserPasswordsRepository userPasswordsRepository,
            UserManager<User> userManager,
            IOptions<UserPasswordsServiceSettings> settings)
        {
            _userPasswordsRepository = userPasswordsRepository;
            _userManager = userManager;
            _settings = settings.Value;
        }

        /// <summary>
        /// Checks new password.
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="newPassword">New password.</param>
        /// <returns>Check result.</returns>
        public async Task<bool> CheckPasswordAsync(User user, string newPassword)
        {
            var userPasswords = await _userPasswordsRepository.GetUserPasswordsByUserIdAsync(user.Id);
            foreach (var userPassword in userPasswords)
            {
                var passwordVerificationResult = _userManager.PasswordHasher.VerifyHashedPassword(
                    user, userPassword.PasswordHash, newPassword);

                if (passwordVerificationResult == PasswordVerificationResult.Success)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if user password expired.
        /// </summary>
        /// <param name="user">User.</param>
        /// <returns>If password expired.</returns>
        public async Task<bool> IsPasswordExpiredAsync(User user)
        {
            if (!_settings.PasswordExpirationPeriod.HasValue)
            {
                return false;
            }

            var userPasswords = await _userPasswordsRepository.GetUserPasswordsByUserIdAsync(user.Id);
            var currentPassword = userPasswords
                .OrderByDescending(x => x.CreateTimeUtc)
                .FirstOrDefault();

            if (currentPassword == null)
            {
                return true;
            }

            return currentPassword.CreateTimeUtc + _settings.PasswordExpirationPeriod.Value < DateTime.UtcNow;
        }

        /// <summary>
        /// Saves new password.
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="userPassword">User password.</param>
        /// <returns>Task.</returns>
        public async Task SaveNewPasswordAsync(User user, UserPassword userPassword)
        {
            var userPasswords = await _userPasswordsRepository.GetUserPasswordsByUserIdAsync(user.Id);

            var userPasswordsToRemove = userPasswords
                .OrderByDescending(x => x.CreateTimeUtc)
                .Skip(_settings.KeepUserPasswordsCount - 1)
                .ToList();

            await _userPasswordsRepository.RemoveRangeAsync(userPasswordsToRemove);

            await _userPasswordsRepository.AddAsync(userPassword);
        }

        /// <summary>
        /// Processes user access failed.
        /// </summary>
        /// <param name="user">User.</param>
        /// <returns>Task.</returns>
        public async Task ProcessAccessFailedAsync(User user)
        {
            var accessFailedCount = await _userManager.GetAccessFailedCountAsync(user);
            if (accessFailedCount >= _settings.MaxAccessFailedCount)
            {
                await _userManager.SetLockoutEnabledAsync(user, true);
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now.Add(_settings.LockoutPeriod));
            }
        }
    }
}


