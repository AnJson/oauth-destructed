using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Models;
using Assignment_Wt1_Oauth.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Assignment_Wt1_Oauth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtHandler _jwtHandler;
        private readonly SessionHandler _sessionHandler;
        private readonly IRequestHandler _requestHandler;

        public AuthService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, JwtHandler jwtHandler, SessionHandler sessionHandler, IRequestHandler requestHandler)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _jwtHandler = jwtHandler;
            _sessionHandler = sessionHandler;
            _requestHandler = requestHandler;
        }

        public OauthAuthRequest GetOauthAuthorizationUri()
        {
            string state = _sessionHandler.GetFromSession(SessionHandler.SessionStorageKey.STATE);
            string code_verifyer = _sessionHandler.GetFromSession(SessionHandler.SessionStorageKey.CODE_VERIFIER);

            if (!string.IsNullOrEmpty(state) && !string.IsNullOrEmpty(code_verifyer))
            {
                OauthAuthRequest authOptions = _configuration.GetSection("Oauthconfig").Get<OauthAuthRequest>();
                authOptions.state = state;
                authOptions.code_challenge = GetCodeChallenge(code_verifyer);
                return authOptions;
            } else
            {
                throw new Exception("Expected InitAuthRequest to have been executed before the GetOauthAuthorizationUri.");
            }
        }

        public async Task<OauthTokenResponse?> GetOauthToken(string code)
        {
            OauthTokenRequest tokenRequestOptions = _configuration.GetSection("Oauthconfig").Get<OauthTokenRequest>();
            tokenRequestOptions.code = code;
            tokenRequestOptions.code_verifier = _sessionHandler.GetFromSession(SessionHandler.SessionStorageKey.CODE_VERIFIER);
            OauthTokenResponse response = await _requestHandler.getTokenRequest(tokenRequestOptions);
            return response;
        }

        public void InitAuthRequest()
        {
            string code_verifier = GetRandomBase64String();
            _sessionHandler.SaveInSession(SessionHandler.SessionStorageKey.CODE_VERIFIER, code_verifier);
            string state = GetRandomBase64String();
            _sessionHandler.SaveInSession(SessionHandler.SessionStorageKey.STATE, state);
        }

        public async Task SignIn(OauthTokenResponse? tokenResponse)
        {
            if (tokenResponse != null)
            {
                IdTokenData idTokenData = _jwtHandler.GetIdTokenData(tokenResponse.id_token);
                int expirationTime = (int)(tokenResponse.expires_in + tokenResponse.created_at);

                _sessionHandler.SaveInSession(SessionHandler.SessionStorageKey.ACCESS_TOKEN, tokenResponse.access_token);
                _sessionHandler.SaveInSession(SessionHandler.SessionStorageKey.REFRESH_TOKEN, tokenResponse.refresh_token);
                _sessionHandler.SaveIntInSession(SessionHandler.SessionStorageKey.TOKEN_EXPIRES, expirationTime);

                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, idTokenData.sub)
                };

                await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims, "sub")));
            }
        }

        private string GetCodeChallenge(string text)
        {
            byte[] sha256Bytes = SHA256.HashData(Encoding.UTF8.GetBytes(text));
            string base64String = Convert.ToBase64String(sha256Bytes);
            return base64String.Replace('+', '-').Replace('/', '_').TrimEnd('=');
        }

        private string GetRandomBase64String()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString())).Replace("=", "y").Replace("+", "-").Replace("/", "_");
        }
    }
}
