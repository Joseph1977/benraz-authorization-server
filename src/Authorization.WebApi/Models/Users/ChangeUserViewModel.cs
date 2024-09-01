using System.ComponentModel.DataAnnotations;

namespace Authorization.WebApi.Models.Users
{
    /// <summary>
    /// Change user view model.
    /// </summary>
    public class ChangeUserViewModel
    {
        /// <summary>
        /// Full name.
        /// </summary>
        [Required]
        public string FullName { get; set; }

        /// <summary>
        /// Phone number.
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}


