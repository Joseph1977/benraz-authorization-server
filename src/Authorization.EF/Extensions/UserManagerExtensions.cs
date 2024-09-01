using Authorization.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authorization.EF.Extensions
{
    /// <summary>
    /// User manager extensions.
    /// </summary>
    public static class UserManagerExtensions
    {
        /// <summary>
        /// Returns user by phone number.
        /// </summary>
        /// <param name="userManager">User manager.</param>
        /// <param name="phoneNumber">Phone number.</param>
        /// <returns>Phone number.</returns>
        public static async Task<User> FindByPhoneNumberAsync(this UserManager<User> userManager, string phoneNumber)
        {
            if (userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager));
            }

            if (string.IsNullOrEmpty(phoneNumber))
            {
                return null;
            }

            var normalizedPhoneNumber = UserPhoneNumber.NormalizePhoneNumber(phoneNumber);
            return await userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == normalizedPhoneNumber);
        }

        /// <summary>
        /// Changes roles of specified user.
        /// </summary>
        /// <param name="userManager">User manager.</param>
        /// <param name="user">User.</param>
        /// <param name="roles">Roles.</param>
        /// <returns>Task.</returns>
        public static async Task ChangeRolesAsync(
            this UserManager<User> userManager, User user, IEnumerable<string> roles)
        {
            if (userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            roles = roles ?? new List<string>();

            var oldRoles = await userManager.GetRolesAsync(user);

            var rolesToRemove = oldRoles.ExceptBy(roles, x => x).ToList();
            await userManager.RemoveFromRolesAsync(user, rolesToRemove);

            var rolesToAdd = roles.ExceptBy(oldRoles, x => x).ToList();
            await userManager.AddToRolesAsync(user, rolesToAdd);
        }

        /// <summary>
        /// Changes claims of specified user.
        /// </summary>
        /// <param name="userManager">User manager.</param>
        /// <param name="user">User.</param>
        /// <param name="claims">Claims.</param>
        /// <returns>Task.</returns>
        public static async Task ChangeClaimsAsync(
            this UserManager<User> userManager, User user, IEnumerable<Claim> claims)
        {
            if (userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            claims = claims ?? new List<Claim>();

            var oldClaims = await userManager.GetClaimsAsync(user);

            var claimsToRemove = oldClaims.ExceptBy(claims, x => new { x.Type, x.Value }).ToList();
            foreach (var claimToRemove in claimsToRemove)
            {
                await userManager.RemoveClaimAsync(user, claimToRemove);
            }

            var claimsToAdd = claims.ExceptBy(oldClaims, x => new { x.Type, x.Value }).ToList();
            foreach (var claimToAdd in claimsToAdd)
            {
                await userManager.AddClaimAsync(user, claimToAdd);
            }
        }

        /// <summary>
        /// Validates password and returns collection of validation errors.
        /// </summary>
        /// <param name="userManager">User manager.</param>
        /// <param name="user">User.</param>
        /// <param name="password">Password.</param>
        /// <returns>Validation errors.</returns>
        public static async Task<IEnumerable<string>> ValidatePasswordAsync(
            this UserManager<User> userManager, User user, string password)
        {
            if (userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            var validationErrors = new List<string>();
            foreach (var passwordValidator in userManager.PasswordValidators)
            {
                var result = await passwordValidator.ValidateAsync(userManager, user, password);
                if (!result.Succeeded)
                {
                    validationErrors.AddRange(result.Errors.Select(x => x.Description));
                }
            }

            return validationErrors;
        }
    }
}


