using Assignment_Wt1_Oauth.Models;

namespace Assignment_Wt1_Oauth.Contracts
{
    public interface IAuthService
    {
        public OauthAuthRequest GetOauthAuthorizationUri();
        public Task<OauthTokenResponse?> GetOauthToken(string code);

        public void InitAuthRequest();
        public Task SignIn(OauthTokenResponse? tokenResponse);

    }
}
