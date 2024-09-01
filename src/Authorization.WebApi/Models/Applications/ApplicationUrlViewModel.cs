using Authorization.Domain.Applications;

namespace Authorization.WebApi.Models.Applications
{
    /// <summary>
    /// Application URL view model.
    /// </summary>
    public class ApplicationUrlViewModel
    {
        /// <summary>
        /// Application URL type code.
        /// </summary>
        public ApplicationUrlTypeCode TypeCode { get; set; }

        /// <summary>
        /// URL value.
        /// </summary>
        public string Url { get; set; }
    }
}


