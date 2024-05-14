using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Authorization.WebApi.Services
{
    /// <summary>
    /// Usage logs service.
    /// </summary>
    public interface IUsageLogsService
    {
        /// <summary>
        /// Logs API usage.
        /// </summary>
        /// <param name="httpContext">HTTP context.</param>
        /// <param name="userName">User name.</param>
        /// <param name="message">Additional message.</param>
        /// <returns>Task.</returns>
        Task LogUsageAsync(HttpContext httpContext, string userName = null, string message = null);
    }
}