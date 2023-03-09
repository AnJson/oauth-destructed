namespace Assignment_Wt1_Oauth.Models
{
    /// <summary>
    /// Wrapper for user activities request.
    /// </summary>
    public class ActivitiesRequest
    {
        public string? EventsUri { get; set; }
        public int? PerPage { get; set; }
        public int? Page { get; set; }


        /// <summary>
        /// Generates the full url for the request.
        /// </summary>
        /// <returns>Full url.</returns>
        public override string ToString()
        {
            return $"{EventsUri}?per_page={PerPage}&page={Page}";
        }
    }
}
