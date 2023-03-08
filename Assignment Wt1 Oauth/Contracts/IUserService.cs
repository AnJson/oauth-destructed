using Assignment_Wt1_Oauth.Models;
using Assignment_Wt1_Oauth.Models.GroupsResponse;

namespace Assignment_Wt1_Oauth.Contracts
{
    public interface IUserService
    {
        public Task<UserProfile?> GetUserProfile();
        public Task<GraphQLGroupsResponse?> GetGroupCollection();
        public Task<UserActivities?> GetActivities(int count);
    }
}
