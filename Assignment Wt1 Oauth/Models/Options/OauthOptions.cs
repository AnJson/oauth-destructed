namespace Assignment_Wt1_Oauth.Models.Options
{
    public class OauthOptions
    {
        public string AppId { get; set; }
        public string RedirectUri { get; set; }
        public string State { get; set; }
        public string RequestedScopes { get; set; }
        public string AppSecret { get; set; }
        public string Code { get; set; }

        public OauthOptions(string appId, string redirectUri, string state, string requestedScopes, string appSecret, string code)
        {
            AppId = appId;
            RedirectUri = redirectUri;
            State = state;
            RequestedScopes = requestedScopes;
            AppSecret = appSecret;
            Code = code;
        }
    }
}
