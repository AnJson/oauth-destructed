using Assignment_Wt1_Oauth.Models;
using Assignment_Wt1_Oauth.Models.GroupsResponse;

namespace Assignment_Wt1_Oauth.Contracts
{
    /// <summary>
    /// Contract for request-handler, referenced in dependency injection to implement dependency inversion principle.
    /// </summary>
    public interface IRequestHandler
    {
        public Task<OauthTokenResponse?> getTokenRequest(OauthTokenRequest options);
        public Task<OauthTokenResponse?> getTokenRequest(OauthRefreshTokenRequest options);
        public Task<UserProfile?> getUserProfile();
        public Task<GraphQLGroupsResponse?> getGroups();
        public Task<UserActivities> getActivites(int count);
    }
}
