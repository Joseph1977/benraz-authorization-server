using Benraz.Infrastructure.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Authorization.Domain.Users
{
    /// <summary>
    ///  Get token by mfa code model.
    /// </summary>
    public class GetTokenByMfaCodeModel
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
        /// Application identifier.
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// Mfa code.
        /// </summary>
        [Required]
        public string Code { get; set; }
    }
}
