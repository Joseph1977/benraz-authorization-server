using System.Collections.Generic;
using System.Security.Claims;

namespace Authorization.Domain.SsoServices
{
    /// <summary>
    /// Base SSO service.
    /// </summary>
    public abstract class SsoServiceBase
    {
        /// <summary>
        /// Adds claim in claims collection.
        /// </summary>
        /// <param name="claims">Claims collection.</param>
        /// <param name="type">New claim type.</param>
        /// <param name="value">New claim value.</param>
        protected void AddClaim(IList<Claim> claims, string type, string value)
        {
            if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(value))
            {
                claims.Add(new Claim(type, value));
            }
        }
    }
}


