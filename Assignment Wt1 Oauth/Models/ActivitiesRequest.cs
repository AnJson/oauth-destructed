namespace Assignment_Wt1_Oauth.Models
{
    public class ActivitiesRequest
    {
        public string? events_uri { get; set; }
        public int? per_page { get; set; }
        public int? page { get; set; }



        public override string ToString()
        {
            return $"{events_uri}?per_page={per_page}&page={page}";
        }
    }
}
