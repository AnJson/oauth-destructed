using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Models;
using Assignment_Wt1_Oauth.Models.GroupsResponse;

namespace Assignment_Wt1_Oauth.Services
{
    public class UserService : IUserService
    {
        private readonly IRequestHandler _requestHandler;


        public UserService(IRequestHandler requestHandler)
        {
            _requestHandler = requestHandler;
        }

        public async Task<GraphQLGroupsResponse?> GetGroupCollection()
        {
            return await _requestHandler.getGroups();
        }

        public async Task<UserProfile?> GetUserProfile()
        {
            return await _requestHandler.getUserProfile();
        }
    }
}
