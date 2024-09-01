using System.Collections.Generic;
using System.Security.Claims;

namespace Authorization.Domain.SsoServices
{
    /// <summary>
    /// SSO authorization result.
    /// </summary>
    public class SsoAuthorizationResult
    {
        /// <summary>
        /// Is authorized.
        /// </summary>
        public bool IsAuthorized { get; set; }

        /// <summary>
        /// Is invalid credentials.
        /// </summary>
        public bool IsInvalidCredentials { get; set; }

        /// <summary>
        /// Is password expired.
        /// </summary>
        public bool IsPasswordExpired { get; set; }

        /// <summary>
        /// Is locked out.
        /// </summary>
        public bool IsLockedOut { get; set; }

        /// <summary>
        /// Claims collection.
        /// </summary>
        public IEnumerable<Claim> Claims { get; set; }

        /// <summary>
        /// Authorization error.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Creates authorization result.
        /// </summary>
        public SsoAuthorizationResult()
        {
            Claims = new List<Claim>();
        }

        /// <summary>
        /// Creates success authorization result.
        /// </summary>
        /// <param name="claims">Claims collection.</param>
        /// <returns>Authorization result.</returns>
        public static SsoAuthorizationResult Authorized(IEnumerable<Claim> claims)
        {
            return new SsoAuthorizationResult
            {
                IsAuthorized = true,
                Claims = claims
            };
        }

        /// <summary>
        /// Creates success authorization result.
        /// </summary>
        /// <param name="claims">Claims collection.</param>
        /// <returns>Authorization result.</returns>
        public static SsoAuthorizationResult PasswordExpired(IEnumerable<Claim> claims)
        {
            var allClaims = new List<Claim>(claims);
            allClaims.Add(new Claim(CustomClaimTypes.IS_PASSWORD_EXPIRED, true.ToString()));

            return new SsoAuthorizationResult
            {
                IsAuthorized = true,
                IsPasswordExpired = true,
                Claims = allClaims
            };
        }

        /// <summary>
        /// Creates invalid credentials authorization result.
        /// </summary>
        /// <param name="error">Error.</param>
        /// <returns>Authorization result.</returns>
        public static SsoAuthorizationResult InvalidCredentials(string error = null)
        {
            return new SsoAuthorizationResult
            {
                IsAuthorized = false,
                IsInvalidCredentials = true,
                Error = error
            };
        }

        /// <summary>
        /// Creates locked out authorization result.
        /// </summary>
        /// <param name="error">Error.</param>
        /// <returns>Authorization result.</returns>
        public static SsoAuthorizationResult LockedOut(string error = null)
        {
            return new SsoAuthorizationResult
            {
                IsAuthorized = false,
                IsLockedOut = true,
                Error = error
            };
        }

        /// <summary>
        /// Creates unauthorized authorization result.
        /// </summary>
        /// <param name="error">Error.</param>
        /// <returns>Authorization result.</returns>
        public static SsoAuthorizationResult Unauthorized(string error = null)
        {
            return new SsoAuthorizationResult
            {
                IsAuthorized = false,
                Error = error
            };
        }
    }
}


