using System.Collections.Generic;
using System.Linq;

namespace Authorization.Domain
{
    /// <summary>
    /// Custom roles.
    /// </summary>
    public static class CustomRoles
    {
        private static readonly string[] _employeeRoles = new string[] { "ADMIN", "EMPLOYEE" };

        /// <summary>
        /// Returns employee roles.
        /// </summary>
        /// <returns>Employee roles.</returns>
        public static IEnumerable<string> GetEmployeeRoles()
        {
            return _employeeRoles.ToList();
        }
    }
}


