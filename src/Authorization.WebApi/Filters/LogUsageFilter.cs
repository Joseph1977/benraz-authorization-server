using Authorization.WebApi.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Authorization.WebApi.Filtes
{
    /// <summary>
    /// Filter for logging service user information.
    /// </summary>
    public class LogUsageFilter : ActionFilterAttribute
    {
        private readonly IUsageLogsService _usageLogsService;

        /// <summary>
        /// Creates filter.
        /// </summary>
        /// <param name="usageLogsService">Usage logs service.</param>
        public LogUsageFilter(IUsageLogsService usageLogsService)
        {
            _usageLogsService = usageLogsService;
        }

        /// <summary>
        /// Executes before action execution.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="next">Action execution delegate.</param>
        /// <returns>Task.</returns>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await _usageLogsService.LogUsageAsync(context.HttpContext);
            await base.OnActionExecutionAsync(context, next);
        }
    }
}


