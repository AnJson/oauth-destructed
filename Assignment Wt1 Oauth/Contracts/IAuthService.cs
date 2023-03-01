using Assignment_Wt1_Oauth.Models.Options;

namespace Assignment_Wt1_Oauth.Contracts
{
    public interface IAuthService
    {
        public OauthAuthRequest GetOauthAuthorizationUri(string code_verifier);
        public OauthTokenResponse GetOauthToken(string code);
        public string GetCodeChallenge(string text);
        public string GetRandomBase64String();
        public void SaveInSession(string key, string code_verifier);

    }
}
