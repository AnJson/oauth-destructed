namespace Assignment_Wt1_Oauth.Models
{
    /// <summary>
    /// Wrapper for user activities request.
    /// </summary>
    public class ActivitiesRequest
    {
        public string? events_uri { get; set; }
        public int? per_page { get; set; }
        public int? page { get; set; }


        /// <summary>
        /// Generates the full url for the request.
        /// </summary>
        /// <returns>Full url.</returns>
        public override string ToString()
        {
            return $"{events_uri}?per_page={per_page}&page={page}";
        }
    }
}
