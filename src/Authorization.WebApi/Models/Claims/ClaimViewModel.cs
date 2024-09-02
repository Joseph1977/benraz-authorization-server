using System;

namespace Authorization.WebApi.Models.Claims
{
    /// <summary>
    /// Claim view model.
    /// </summary>
    public class ClaimViewModel
    {
        /// <summary>
        /// Claim identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Claim type.
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Claim value.
        /// </summary>
        public string? Value { get; set; }
    }
}


