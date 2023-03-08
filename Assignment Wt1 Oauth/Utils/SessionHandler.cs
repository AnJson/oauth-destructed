using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System.Drawing;
using System.Security.Claims;

namespace Assignment_Wt1_Oauth.Utils
{
    public class SessionHandler : ISessionHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IJwtHandler _jwtHandler;

        public SessionHandler(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IJwtHandler jwtHandler)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _jwtHandler = jwtHandler;
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

        public async Task SignOut()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(_configuration.GetValue<string>("session_cookie"));
            await _httpContextAccessor.HttpContext.SignOutAsync();
        }

        public async Task SignIn(OauthTokenResponse? tokenResponse)
        {
            if (tokenResponse != null)
            {
                IdTokenData idTokenData = _jwtHandler.GetIdTokenData(tokenResponse.id_token);
                int expirationTime = (int)(tokenResponse.expires_in + tokenResponse.created_at);

                SaveInSession(SessionStorageKey.ACCESS_TOKEN, tokenResponse.access_token);
                SaveInSession(SessionStorageKey.REFRESH_TOKEN, tokenResponse.refresh_token);
                SaveIntInSession(SessionStorageKey.TOKEN_EXPIRES, expirationTime);

                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, idTokenData.sub)
                };

                await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims, "sub")));
            }
        }
    }
}
