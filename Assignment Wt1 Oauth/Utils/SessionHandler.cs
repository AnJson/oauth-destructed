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
    /// <summary>
    /// Handles session-storage.
    /// </summary>
    public class SessionHandler : ISessionHandler
    {
        /// <summary>
        /// Injected service.
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Injected service.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Injected service.
        /// </summary>
        private readonly IJwtHandler _jwtHandler;

        /// <summary>
        /// Constructor recieving injected services.
        /// </summary>
        /// <param name="httpContextAccessor">Service to inject.</param>
        /// <param name="configuration">Service to inject.</param>
        /// <param name="jwtHandler">Service to inject.</param>
        public SessionHandler(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IJwtHandler jwtHandler)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _jwtHandler = jwtHandler;
        }

        /// <summary>
        /// Enum for available session keys.
        /// Used in GetSessionStorageKey to get the key as a string.
        /// </summary>
        public enum SessionStorageKey
        {
            ACCESS_TOKEN,
            REFRESH_TOKEN,
            STATE,
            CODE_VERIFIER,
            TOKEN_EXPIRES
        }

        /// <summary>
        /// Factory-method to get session key as a string.
        /// </summary>
        /// <param name="key">Enum key to get as string.</param>
        /// <returns>Session key as string.</returns>
        /// <exception cref="ArgumentException">If invalid argument.</exception>
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

        /// <summary>
        /// Saves string in session storage.
        /// </summary>
        /// <param name="key">Key name.</param>
        /// <param name="value">The value to store for the key.</param>
        public void SaveInSession(SessionStorageKey key, string value)
        {
            _httpContextAccessor.HttpContext.Session.SetString(GetSessionStorageKey(key), value);
        }

        /// <summary>
        /// Saves int in session storage.
        /// </summary>
        /// <param name="key">Key name.</param>
        /// <param name="value">The value to store for the key.</param>
        public void SaveIntInSession(SessionStorageKey key, int value)
        {
            _httpContextAccessor.HttpContext.Session.SetInt32(GetSessionStorageKey(key), value);
        }

        /// <summary>
        /// Read string from session storage.
        /// </summary>
        /// <param name="key">The key to read.</param>
        public string? GetFromSession(SessionStorageKey key)
        {
            return _httpContextAccessor.HttpContext.Session.GetString(GetSessionStorageKey(key));
        }

        /// <summary>
        /// Read int from session storage.
        /// </summary>
        /// <param name="key">The key to read.</param>
        public int? GetIntFromSession(SessionStorageKey key)
        {
            return _httpContextAccessor.HttpContext.Session.GetInt32(GetSessionStorageKey(key));
        }

        /// <summary>
        /// Clear session storage and deletes cookies.
        /// </summary>
        /// <returns></returns>
        public async Task SignOut()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(_configuration.GetValue<string>("session_cookie"));
            await _httpContextAccessor.HttpContext.SignOutAsync();
        }

        /// <summary>
        /// Saves token-data in session and creates auth-cookie.
        /// </summary>
        /// <param name="tokenResponse">Wrapper model for the oauth token response, to read the id_token from.</param>
        /// <returns></returns>
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
