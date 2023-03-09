namespace Assignment_Wt1_Oauth.Models
{
    /// <summary>
    /// Wrapper for query to be sent in refresh-token request.
    /// </summary>
    public class OauthRefreshTokenRequest
    {
        public string? clientId { get; set; }
        public string? refreshToken { get; set; }
        public string? grantType { get; } = "refresh_token";
        public string? redirectUri { get; set; }
        public string? codeVerifier { get; set; }
        public string? clientSecret { get; set; }
    }
}
