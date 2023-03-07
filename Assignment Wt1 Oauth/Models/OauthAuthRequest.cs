namespace Assignment_Wt1_Oauth.Models
{
    public class OauthAuthRequest
    {
        public string? client_id { get; set; }
        public string? redirect_uri { get; set; }
        public string? scope { get; set; }
        public string? authorization_uri { get; set; }
        public string? code_challenge { get; set; }
        public string? state { get; set; }


        public override string ToString()
        {
            return $"{authorization_uri}?client_id={client_id}&redirect_uri={redirect_uri}&response_type=code&state={state}&scope={scope}&code_challenge={code_challenge}&code_challenge_method=S256";
        }
    }
}
