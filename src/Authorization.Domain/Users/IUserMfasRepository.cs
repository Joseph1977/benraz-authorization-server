using Benraz.Infrastructure.Common.Repositories;
using Benraz.Infrastructure.Domain.Common;
using System;
using System.Threading.Tasks;

namespace Authorization.Domain.Users
{
    /// <summary>
    /// User mfas repository.
    /// </summary>
    public interface IUserMfasRepository : IRepository<Guid, UserMfa>
    {
        /// <summary>
        /// Returns user mfa by user identifier and code type.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="mfaCode">Mfa code.</param>
        /// <returns>User mfa.</returns>
        Task<UserMfa> GetUserMfaByUserIdAndCodeTypeAsync(string userId, MfaCode mfaCode);
    }
}
