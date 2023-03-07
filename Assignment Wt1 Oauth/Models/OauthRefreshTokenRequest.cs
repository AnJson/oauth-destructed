namespace Assignment_Wt1_Oauth.Models
{
    public class OauthRefreshTokenRequest
    {
        public string? client_id { get; set; }
        public string? refresh_token { get; set; }
        public string? grant_type { get; } = "refresh_token";
        public string? redirect_uri { get; set; }
        public string? code_verifier { get; set; }
        // public string? client_secret { get; set; }
    }
}
