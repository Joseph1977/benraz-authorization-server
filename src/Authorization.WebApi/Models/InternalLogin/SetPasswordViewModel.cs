using Authorization.Domain.Users;
using System.ComponentModel.DataAnnotations;

namespace Authorization.WebApi.Models.InternalLogin
{
    /// <summary>
    /// Set password view model.
    /// </summary>
    public class SetPasswordViewModel
    {
        /// <summary>
        /// Reset password code.
        /// </summary>
        [Required]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// The new password.
        /// </summary>
        [Required]
        [RegularExpression(UserPassword.PASSWORD_REGEX)]
        public string NewPassword { get; set; } = string.Empty;
    }
}


