using Assignment_Wt1_Oauth.Models;
using static Assignment_Wt1_Oauth.Utils.SessionHandler;

namespace Assignment_Wt1_Oauth.Contracts
{
    /// <summary>
    /// Contract for session-handler, referenced in dependency injection to implement dependency inversion principle.
    /// </summary>
    public interface ISessionHandler
    {
        public string GetSessionStorageKey(SessionStorageKey key);
        public void SaveInSession(SessionStorageKey key, string value);
        public void SaveIntInSession(SessionStorageKey key, int value);
        public string? GetFromSession(SessionStorageKey key);
        public int? GetIntFromSession(SessionStorageKey key);
        public Task SignOut();
        public Task SignIn(OauthTokenResponse? tokenResponse);
    }
}
