using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Models;
using Assignment_Wt1_Oauth.Models.GroupsResponse;
using Assignment_Wt1_Oauth.Utils;

namespace Assignment_Wt1_Oauth.Services
{
    /// <summary>
    /// Service that handles user.
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// Injected service.
        /// </summary>
        private readonly IRequestHandler _requestHandler;


        /// <summary>
        /// Constructor recieving injected services.
        /// </summary>
        /// <param name="requestHandler">Injected service.</param>
        public UserService(IRequestHandler requestHandler)
        {
            _requestHandler = requestHandler;
        }

        /// <summary>
        /// Fetches user-profile from gitlab.
        /// </summary>
        /// <returns>Wrapper model for profile-response.</returns>
        public async Task<UserProfile?> GetUserProfile()
        {
            return await _requestHandler.getUserProfile();
        }

        /// <summary>
        /// Fetches groups and projects data from GraphQL endpoint.
        /// </summary>
        /// <returns>Wrapper model for the response.</returns>
        public async Task<GraphQLGroupsResponse?> GetGroupCollection()
        {
            return await _requestHandler.getGroups();
        }

        /// <summary>
        /// Fetches the current users lates activities.
        /// </summary>
        /// <param name="count">Number of activities to fetch.</param>
        /// <returns>Wrapper model for the activities.</returns>
        public async Task<UserActivities?> GetActivities(int count)
        {
            return await _requestHandler.getActivites(count);
        }
    }
}
