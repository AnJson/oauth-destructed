namespace Assignment_Wt1_Oauth.Models.Options
{
    public class OauthTokenResponse
    {
        public string? access_token { get; set; }
        public string? refresh_token { get; set; }
        public string? token_type { get; set; }
        public string? id_token { get; set; }
        public int? expires_in { get; set; }
        public int? created_at { get; set; }
    }
}
