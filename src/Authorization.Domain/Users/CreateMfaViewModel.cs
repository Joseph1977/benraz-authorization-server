using Benraz.Infrastructure.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Authorization.Domain.Users
{
    /// <summary>
    ///  Create mfa view model.
    /// </summary>
    public class CreateMfaViewModel
    {
        /// <summary>
        /// User email.
        /// </summary>
        [Required]
        public string UserEmail { get; set; }

        /// <summary>
        /// Action type.
        /// </summary>
        [Required]
        public MfaCode ActionType { get; set; }

        /// <summary>
        /// Mode to send the code phone/email.
        /// </summary>
        [Required]
        public MfaMode Mode { get; set; }

        /// <summary>
        /// Application identifier.
        /// </summary>
        public string ApplicationId { get; set; }
    }
}
