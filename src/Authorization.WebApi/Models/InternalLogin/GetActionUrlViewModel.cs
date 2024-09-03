using Benraz.Infrastructure.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Authorization.WebApi.Models.InternalLogin
{
    /// <summary>
    /// Set initial password view  model.
    /// </summary>
    public class GetActionUrlViewModel
    {
        /// <summary>
        /// User name.
        /// </summary>
        [Required]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Not send email.
        /// </summary>
        public bool NotSendEmail { get; set; }

        /// <summary>
        /// Action type.
        /// </summary>
        public EmailActionType ActionType { get; set; }
    }
}
