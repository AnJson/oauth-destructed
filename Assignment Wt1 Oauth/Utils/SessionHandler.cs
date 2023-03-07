using Assignment_Wt1_Oauth.Contracts;
using System.Drawing;

namespace Assignment_Wt1_Oauth.Utils
{
    public class SessionHandler : ISessionHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public enum SessionStorageKey
        {
            ACCESS_TOKEN,
            REFRESH_TOKEN,
            STATE,
            CODE_VERIFIER,
            TOKEN_EXPIRES
        }

        public string GetSessionStorageKey(SessionStorageKey key)
        {
            switch (key)
            {
                case SessionStorageKey.ACCESS_TOKEN:
                    return "access_token";
                case SessionStorageKey.REFRESH_TOKEN:
                    return "refresh_token";
                case SessionStorageKey.STATE:
                    return "state";
                case SessionStorageKey.CODE_VERIFIER:
                    return "code_verifier";
                case SessionStorageKey.TOKEN_EXPIRES:
                    return "token_expires";
                default:
                    throw new ArgumentException("Invalid key");
            }
        }

        public void SaveInSession(SessionStorageKey key, string value)
        {
            _httpContextAccessor.HttpContext.Session.SetString(GetSessionStorageKey(key), value);
        }

        public void SaveIntInSession(SessionStorageKey key, int value)
        {
            _httpContextAccessor.HttpContext.Session.SetInt32(GetSessionStorageKey(key), value);
        }

        public string? GetFromSession(SessionStorageKey key)
        {
            return _httpContextAccessor.HttpContext.Session.GetString(GetSessionStorageKey(key));
        }

        public int? GetIntFromSession(SessionStorageKey key)
        {
            return _httpContextAccessor.HttpContext.Session.GetInt32(GetSessionStorageKey(key));
        }
    }
}
