using System.Drawing;

namespace Assignment_Wt1_Oauth.Utils
{
    public class SessionHandler
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
            EMAIL,
            USERID,
            STATE,
            CODE_VERIFIER
        }

        public string GetSessionStorageKey(SessionStorageKey key)
        {
            switch (key)
            {
                case SessionStorageKey.ACCESS_TOKEN:
                    return "access_token";
                case SessionStorageKey.REFRESH_TOKEN:
                    return "refresh_token";
                case SessionStorageKey.EMAIL:
                    return "email";
                case SessionStorageKey.USERID:
                    return "userid";
                case SessionStorageKey.STATE:
                    return "state";
                case SessionStorageKey.CODE_VERIFIER:
                    return "code_verifier";
                default:
                    throw new ArgumentException("Invalid key");
            }
        }

        public void SaveInSession(SessionStorageKey key, string value)
        {
            _httpContextAccessor.HttpContext.Session.SetString(GetSessionStorageKey(key), value);
        }

        public string? GetFromSession(SessionStorageKey key)
        {
            return _httpContextAccessor.HttpContext.Session.GetString(GetSessionStorageKey(key));
        }
    }
}
