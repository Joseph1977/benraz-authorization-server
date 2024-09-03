namespace Authorization.Domain.Applications
{
    /// <summary>
    /// Applications query.
    /// </summary>
    public class ApplicationsQuery
    {
        /// <summary>
        /// Filter.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Sort by parameter.
        /// </summary>
        public ApplicationsQueryParameter SortBy { get; set; }

        /// <summary>
        /// Is sort descending.
        /// </summary>
        public bool SortDesc { get; set; }

        /// <summary>
        /// Page number.
        /// </summary>
        public int PageNo { get; set; }

        /// <summary>
        /// Page size.
        /// </summary>
        public int PageSize { get; set; }
    }
}


