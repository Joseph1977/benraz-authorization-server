using Benraz.Infrastructure.Domain.Authorization;
using System;
using System.Threading.Tasks;

namespace Authorization.Domain
{
    /// <summary>
    /// Authorization service.
    /// </summary>
    public interface IAuthorizationService
    {
        /// <summary>
        /// Creates and returns authorization URL.
        /// </summary>
        /// <param name="applicationId">Application identifier.</param>
        /// <param name="ssoProviderCode">SSO provider code.</param>
        /// <param name="returnUrl">Return URL.</param>
        /// <returns>Authorization URL.</returns>
        Task<string> CreateAuthorizationUrlAsync(
            Guid applicationId, SsoProviderCode ssoProviderCode, string returnUrl);

        /// <summary>
        /// Completes authorization code authorization flow and returns access token.
        /// </summary>
        /// <param name="applicationId">Application identifier.</param>
        /// <param name="ssoProviderCode">SSO provider code.</param>
        /// <param name="code">Authorization code.</param>
        /// <returns>Access token result.</returns>
        Task<AccessTokenResult> AuthorizeAsync(Guid applicationId, SsoProviderCode ssoProviderCode, string code);

        /// <summary>
        /// Completes resource owner password credentials authorization flow and returns access token.
        /// </summary>
        /// <param name="applicationId">Application identifier.</param>
        /// <param name="ssoProviderCode">SSO provider code.</param>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        /// <returns>Access token result.</returns>
        Task<AccessTokenResult> AuthorizeAsync(
            Guid applicationId, SsoProviderCode ssoProviderCode, string username, string password);

        /// <summary>
        /// Processes authorization flow using an existing access token and returns a new one.
        /// </summary>
        /// <param name="applicationId">Application identifier.</param>
        /// <param name="accessToken">Access token.</param>
        /// <returns>Access token result.</returns>
        Task<AccessTokenResult> AuthorizeAsync(Guid applicationId, string accessToken);

        /// <summary>
        /// Creates and returns callback URL.
        /// </summary>
        /// <param name="applicationId">Application identifier.</param>
        /// <param name="accessToken">Access token.</param>
        /// <param name="returnUrl">Return URL.</param>
        /// <returns>Callback URL.</returns>
        Task<string> CreateSuccessCallbackUrlAsync(Guid applicationId, string accessToken, string returnUrl);

        /// <summary>
        /// Creates and returns URL for callback with error.
        /// </summary>
        /// <param name="applicationId">Application identifier.</param>
        /// <param name="error">Error.</param>
        /// <returns>Callback URL.</returns>
        Task<string> CreateErrorCallbackUrlAsync(Guid applicationId, string error);
    }
}


