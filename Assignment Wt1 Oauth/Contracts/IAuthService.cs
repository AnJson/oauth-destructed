using Assignment_Wt1_Oauth.Models;

namespace Assignment_Wt1_Oauth.Contracts
{
    /// <summary>
    /// Contract for auth-service
    /// </summary>
    public interface IAuthService
    {
        public OauthAuthRequest GetOauthAuthorizationUri();
        public Task<OauthTokenResponse?> GetOauthToken(string code);
        public Task SignIn(OauthTokenResponse? tokenResponse);
        public Task SignOut();


    }
}
