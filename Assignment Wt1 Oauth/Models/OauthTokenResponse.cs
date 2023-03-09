namespace Assignment_Wt1_Oauth.Models
{
    /// <summary>
    /// Wrapper that extracts the needed data from token-response.
    /// </summary>
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
