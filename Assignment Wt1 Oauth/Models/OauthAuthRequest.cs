namespace Assignment_Wt1_Oauth.Models
{
    /// <summary>
    /// Wrapper for the authentication request.
    /// </summary>
    public class OauthAuthRequest
    {
        public string? clientId { get; set; }
        public string? redirectUri { get; set; }
        public string? scope { get; set; }
        public string? authorizationUri { get; set; }
        public string? codeChallenge { get; set; }
        public string? state { get; set; }

        /// <summary>
        /// Generates the full url for the request.
        /// </summary>
        /// <returns>Full url.</returns>
        public override string ToString()
        {
            return $"{authorizationUri}?client_id={clientId}&redirect_uri={redirectUri}&response_type=code&state={state}&scope={scope}&code_challenge={codeChallenge}&code_challenge_method=S256";
        }
    }
}
