namespace Assignment_Wt1_Oauth.Models.Options
{
    public class OauthAuthorizationOptions
    {
        public string? AppId { get; set; }
        public string? RedirectUri { get; set; }
        public string? RequestedScopes { get; set; }
        public string? AuthorizationUri { get; set; }

        public override string ToString()
        {
            return $"{AuthorizationUri}?client_id={AppId}&redirect_uri={RedirectUri}&response_type=code&scope={RequestedScopes}";
        }
    }
}
