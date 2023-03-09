using Assignment_Wt1_Oauth.Models;
using Assignment_Wt1_Oauth.Models.GroupsResponse;

namespace Assignment_Wt1_Oauth.Contracts
{
    /// <summary>
    /// Contract for user-service, referenced in dependency injection to implement dependency inversion principle.
    /// </summary>
    public interface IUserService
    {
        public Task<UserProfile?> GetUserProfile();
        public Task<GraphQLGroupsResponse?> GetGroupCollection();
        public Task<UserActivities?> GetActivities(int count);
    }
}
