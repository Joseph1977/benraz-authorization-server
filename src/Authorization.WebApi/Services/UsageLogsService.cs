using Authorization.Domain.UsageLogs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Benraz.Infrastructure.Common.AccessControl;
using Benraz.Infrastructure.Common.DataRedundancy;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.WebApi.Services
{
    /// <summary>
    /// Usage logs service.
    /// </summary>
    public class UsageLogsService : IUsageLogsService
    {
        private readonly IUsageLogsRepository _usageLogsRepository;
        private readonly IDrChecker _drChecker;
        private readonly ILogger<UsageLogsService> _logger;

        /// <summary>
        /// Creates filter.
        /// </summary>
        /// <param name="usageLogsRepository">Usage logs repository.</param>
        /// <param name="drChecker">Data redundancy checker.</param>
        /// <param name="logger">Logger.</param>
        public UsageLogsService(
            IUsageLogsRepository usageLogsRepository, IDrChecker drChecker, ILogger<UsageLogsService> logger)
        {
            _usageLogsRepository = usageLogsRepository;
            _drChecker = drChecker;
            _logger = logger;
        }

        /// <summary>
        /// Logs API usage.
        /// </summary>
        /// <param name="httpContext">HTTP context.</param>
        /// <param name="userName">User name.</param>
        /// <param name="message">Additional message.</param>
        /// <returns>Task.</returns>
        public async Task LogUsageAsync(HttpContext httpContext, string? userName = null, string? message = null)
        {
            if (!_drChecker.IsActiveDR())
            {
                return;
            }

            try
            {
                var log = new UsageLog
                {
                    UserName = userName ?? GetUserId(httpContext),
                    IPAddress = GetIPAddress(httpContext),
                    Action = GetAction(httpContext, message)
                };

                await _usageLogsRepository.AddAsync(log);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while logging API usage: {ex}");
            }
        }

        private string? GetUserId(HttpContext httpContext)
        {
            return httpContext.User?.Claims?.FirstOrDefault(x => x.Type == CommonClaimTypes.USER_ID)?.Value;
        }

        private string? GetIPAddress(HttpContext httpContext)
        {
            return httpContext.Connection.RemoteIpAddress?.ToString();
        }

        private string GetAction(HttpContext httpContext, string? message)
        {
            var action = $"{httpContext.Request.Method} {httpContext.Request.Path}";
            if (!string.IsNullOrEmpty(message))
            {
                action += $". {message}";
            }

            return action;
        }
    }
}


