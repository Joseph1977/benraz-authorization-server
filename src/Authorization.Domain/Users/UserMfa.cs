using Benraz.Infrastructure.Common.EntityBase;
using Benraz.Infrastructure.Domain.Common;
using System;

namespace Authorization.Domain.Users
{
    /// <summary>
    /// User mfa.
    /// </summary>
    public class UserMfa : AggregateRootBase<Guid>
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Type.
        /// </summary>
        public MfaCode Type { get; set; }

        /// <summary>
        /// Expiration date.
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Send through.
        /// </summary>
        public string SendThrough { get; set; }

        /// <summary>
        /// Is send success.
        /// </summary>
        public bool? IsSendSuccess { get; set; }

        /// <summary>
        /// Send error.
        /// </summary>
        public string SendError { get; set; }

        /// <summary>
        /// Is code consumed.
        /// </summary>
        public bool? IsCodeConsumed { get; set; }
    }
}
