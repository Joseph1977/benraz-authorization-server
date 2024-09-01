using Authorization.Domain.Users;
using System.ComponentModel.DataAnnotations;

namespace Authorization.WebApi.Models.InternalLogin
{
    /// <summary>
    /// Change password view model.
    /// </summary>
    public class ChangePasswordViewModel
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        [Required]
        public string OldPassword { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        [Required]
        [RegularExpression(UserPassword.PASSWORD_REGEX)]
        public string NewPassword { get; set; }
    }
}


