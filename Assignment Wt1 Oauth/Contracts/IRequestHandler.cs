using Assignment_Wt1_Oauth.Models;

namespace Assignment_Wt1_Oauth.Contracts
{
    public interface IRequestHandler
    {
        public Task<OauthTokenResponse?> getTokenRequest(OauthTokenRequest options);
        public Task<OauthTokenResponse?> getTokenRequest(OauthRefreshTokenRequest options);

    }
}
