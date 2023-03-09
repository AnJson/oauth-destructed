﻿namespace Assignment_Wt1_Oauth.Models
{
    /// <summary>
    /// Wrapper for query to be sent in token request.
    /// </summary>
    public class OauthTokenRequest
    {
        public string? client_id { get; set; }
        public string? code { get; set; }
        public string? grant_type { get; } = "authorization_code";
        public string? redirect_uri { get; set; }
        public string? code_verifier { get; set; }
        public string? client_secret { get; set; }
    }
}
