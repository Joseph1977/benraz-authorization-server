using System.ComponentModel.DataAnnotations;

namespace Authorization.WebApi.Models.InternalLogin
{
    /// <summary>
    /// Confirm email view model.
    /// </summary>
    public class ConfirmEmailViewModel
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        [Required] 
        public string UserId { get; set; }

        /// <summary>
        /// Email confirmation token.
        /// </summary>
        [Required] 
        public string Token { get; set; }
    }
}


