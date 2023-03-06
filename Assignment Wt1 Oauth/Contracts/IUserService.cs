using Assignment_Wt1_Oauth.Models;

namespace Assignment_Wt1_Oauth.Contracts
{
    public interface IUserService
    {
        public Task<UserProfile?> GetUserProfile();
        public Task<GroupCollection> GetGroupCollection();
    }
}
