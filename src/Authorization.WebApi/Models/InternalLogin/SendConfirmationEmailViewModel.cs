using System.ComponentModel.DataAnnotations;

namespace Authorization.WebApi.Models.InternalLogin
{
    /// <summary>
    /// Send confirmation email view model.
    /// </summary>
    public class SendConfirmationEmailViewModel
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        [Required] 
        public string UserId { get; set; }
    }
}


