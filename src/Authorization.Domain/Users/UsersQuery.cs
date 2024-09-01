using Benraz.Infrastructure.Domain.Authorization;
using System.Collections.Generic;

namespace Authorization.Domain.Users
{
    /// <summary>
    /// Users query.
    /// </summary>
    public class UsersQuery
    {
        /// <summary>
        /// Filter.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Full name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Phone number.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Email confirmed.
        /// </summary>
        public bool? EmailConfirmed { get; set; }

        /// <summary>
        /// Phone number confirmed.
        /// </summary>
        public bool? PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Status identifiers.
        /// </summary>
        public ICollection<UserStatusCode> StatusIds { get; set; }

        /// <summary>
        /// Whether to use OR instead of AND.
        /// </summary>
        public bool Any { get; set; }

        /// <summary>
        /// Sort by parameter.
        /// </summary>
        public UsersQueryParameter SortBy { get; set; }

        /// <summary>
        /// Is sort descending.
        /// </summary>
        public bool SortDesc { get; set; }

        /// <summary>
        /// Page number.
        /// </summary>
        public int PageNo { get; set; }

        /// <summary>
        /// Page size.
        /// </summary>
        public int PageSize { get; set; }
    }
}


