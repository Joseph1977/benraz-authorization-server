using Authorization.Domain.Users;
using System.ComponentModel.DataAnnotations;

namespace Authorization.WebApi.Models.InternalLogin
{
    /// <summary>
    /// Sign up view model.
    /// </summary>
    public class SignUpViewModel
    {
        /// <summary>
        /// User name.
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// First name.
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name.
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        [Required]
        [RegularExpression(UserPassword.PASSWORD_REGEX)]
        public string Password { get; set; }

        /// <summary>
        /// Phone number.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Send confirmation email, if false, 
        /// server will not send the confirmation email to user.
        /// </summary>
        public bool SendConfirmationEmail { get; set; } = true;
    }
}


