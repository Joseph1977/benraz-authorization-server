using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Authorization.WebApi.Models.Users
{
    /// <summary>
    /// Create user view model.
    /// </summary>
    public class CreateUserViewModel
    {
        /// <summary>
        /// Full name.
        /// </summary>
        [Required]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Email.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Phone number.
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Password to set.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Roles.
        /// </summary>
        public ICollection<string>? Roles { get; set; }

        /// <summary>
        /// Claims.
        /// </summary>
        public ICollection<UserClaimViewModel>? Claims { get; set; }

        /// <summary>
        /// Creates view model.
        /// </summary>
        public CreateUserViewModel()
        {
            Roles = new List<string>();
            Claims = new List<UserClaimViewModel>();
        }
    }
}


