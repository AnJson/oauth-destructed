using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Models;
using Assignment_Wt1_Oauth.Utils;

namespace Assignment_Wt1_Oauth.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SessionHandler _sessionHandler;


        public UserService(IHttpContextAccessor httpContextAccessor, SessionHandler sessionHandler)
        {
            _httpContextAccessor = httpContextAccessor;
            _sessionHandler = sessionHandler;
        }
        public UserProfile GetUserProfile()
        {
            UserProfile profile = new UserProfile();
            profile.userid = _sessionHandler.GetFromSession(SessionHandler.SessionStorageKey.USERID);
            profile.email = _sessionHandler.GetFromSession(SessionHandler.SessionStorageKey.EMAIL);
            return profile;
        }
    }
}
