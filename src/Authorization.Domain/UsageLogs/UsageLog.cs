using Benraz.Infrastructure.Common.EntityBase;

namespace Authorization.Domain.UsageLogs
{
    /// <summary>
    /// Usage log.
    /// </summary>
    public class UsageLog : AggregateRootBase<int>
    {
        /// <summary>
        /// Client user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// CLient IP address.
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// Action accessed.
        /// </summary>
        public string Action { get; set; }
    }
}


