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
        private readonly HttpClient _httpClient;
        private readonly JwtHandler _jwtHandler;
        private readonly SessionHandler _sessionHandler;

        public AuthService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, HttpClient httpClient, JwtHandler jwtHandler, SessionHandler sessionHandler)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
            _jwtHandler = jwtHandler;
            _sessionHandler = sessionHandler;
        }

        public OauthAuthRequest GetOauthAuthorizationUri()
        {
            string state = _sessionHandler.GetFromSession(SessionHandler.SessionStorageKey.STATE);
            string code_verifyer = _sessionHandler.GetFromSession(SessionHandler.SessionStorageKey.CODE_VERIFIER);

            if (!string.IsNullOrEmpty(state) && !string.IsNullOrEmpty(code_verifyer))
            {
                OauthAuthRequest authOptions = _configuration.GetSection("Oauthconfig").Get<OauthAuthRequest>();
                authOptions.State = state;
                authOptions.CodeChallenge = GetCodeChallenge(code_verifyer);
                return authOptions;
            } else
            {
                throw new Exception("Expected InitAuthRequest to have been executed before the GetOauthAuthorizationUri.");
            }
        }

        public async Task<OauthTokenResponse?> GetOauthToken(string code)
        {
            OauthTokenRequest tokenRequestOptions = new OauthTokenRequest();
            tokenRequestOptions.redirect_uri = _configuration.GetValue<string>("Oauthconfig:RedirectUri");
            tokenRequestOptions.client_id = _configuration.GetValue<string>("Oauthconfig:AppId");
            tokenRequestOptions.client_secret = _configuration.GetValue<string>("Oauthconfig:AppSecret");
            tokenRequestOptions.code = code;
            tokenRequestOptions.grant_type = "authorization_code";
            tokenRequestOptions.code_verifier = _sessionHandler.GetFromSession(SessionHandler.SessionStorageKey.CODE_VERIFIER);
            OauthTokenResponse response = await getTokenRequest(tokenRequestOptions);
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
                _sessionHandler.SaveInSession(SessionHandler.SessionStorageKey.ACCESS_TOKEN, tokenResponse.access_token);
                _sessionHandler.SaveInSession(SessionHandler.SessionStorageKey.REFRESH_TOKEN, tokenResponse.refresh_token);
                // Is groups needed??????????????????????????????????????????????????????????????????????????????????????????????????????????????????

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

        private async Task<OauthTokenResponse?> getTokenRequest(OauthTokenRequest options)
        {
            // Transform OauthTokenRequest to enumerable keyvaluepair.
            IEnumerable<KeyValuePair<string, string?>> optionsKeyValuePairs = options.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(string))
                .ToDictionary(p => p.Name, p => (string?)p.GetValue(options));

            FormUrlEncodedContent content = new FormUrlEncodedContent(optionsKeyValuePairs);
            string tokenUri = _configuration.GetValue<string>("Oauthconfig:TokenUri");
            HttpResponseMessage response = await _httpClient.PostAsync(tokenUri, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Token postrequest failed with statuscode {response.StatusCode}");
            }

            string responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<OauthTokenResponse>(responseContent);
        }
    }
}
