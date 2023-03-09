namespace Assignment_Wt1_Oauth.Models
{
    /// <summary>
    /// Wrapper for query to be sent in token request.
    /// </summary>
    public class OauthTokenRequest
    {
        public string? clientId { get; set; }
        public string? code { get; set; }
        public string? grantType { get; } = "authorization_code";
        public string? redirectUri { get; set; }
        public string? codeVerifier { get; set; }
        public string? clientSecret { get; set; }
    }
}
