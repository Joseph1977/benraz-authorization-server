using System.ComponentModel.DataAnnotations;

namespace Authorization.WebApi.Models.InternalLogin
{
    /// <summary>
    /// Login view model.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// State.
        /// </summary>
        [Required]
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// User name.
        /// </summary>
        [Required]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Password.
        /// </summary>
        [Required]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Is mfa verified.
        /// </summary>
        public bool? isMfaVerified { get; set; }
    }
}


