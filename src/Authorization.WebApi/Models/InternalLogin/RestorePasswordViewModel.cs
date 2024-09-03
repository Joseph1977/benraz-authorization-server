using System.ComponentModel.DataAnnotations;

namespace Authorization.WebApi.Models.InternalLogin
{
    /// <summary>
    /// Restore password view model.
    /// </summary>
    public class RestorePasswordViewModel
    {
        /// <summary>
        /// User name.
        /// </summary>
        [Required]
        public string Username { get; set; } = string.Empty;
    }
}


