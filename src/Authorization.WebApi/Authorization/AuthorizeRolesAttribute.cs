using Microsoft.AspNetCore.Authorization;

namespace Authorization.WebApi.Attributes
{
    /// <summary>
    /// Authorize by having at least one of the roles.
    /// </summary>
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Creates attribute.
        /// </summary>
        /// <param name="roles">Roles. Presence of any of the roles grants access.</param>
        public AuthorizeRolesAttribute(params string[] roles)
        {
            Roles = string.Join(",", roles);
        }
    }
}


