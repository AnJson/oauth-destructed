namespace Assignment_Wt1_Oauth.Models.Options
{
    public class OauthAuthRequest
    {
        public string? AppId { get; set; }
        public string? RedirectUri { get; set; }
        public string? RequestedScopes { get; set; }
        public string? AuthorizationUri { get; set; }
        public string? State { get; set; }
        public string? CodeChallenge { get; set; }

        public override string ToString()
        {
            return $"{AuthorizationUri}?client_id={AppId}&redirect_uri={RedirectUri}&response_type=code&state={State}&scope={RequestedScopes}&code_challenge={CodeChallenge}&code_challenge_method=S256";
        }
    }
}
