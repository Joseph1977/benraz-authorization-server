using System.Threading.Tasks;

namespace Authorization.Domain.ApplicationTokens
{
    /// <summary>
    /// Application tokens service.
    /// </summary>
    public interface IApplicationTokensService
    {
        /// <summary>
        /// Creates application token.
        /// </summary>
        /// <param name="request">Create application token request.</param>
        /// <returns>Application token.</returns>
        Task<ApplicationToken> CreateAsync(CreateApplicationToken request);
    }
}


