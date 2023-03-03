using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Models.Options;
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

        public AuthService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
        }

        public OauthAuthRequest GetOauthAuthorizationUri()
        {
            string state = _httpContextAccessor.HttpContext.Session.GetString("state");
            string code_verifyer = _httpContextAccessor.HttpContext.Session.GetString("code_verifier");

            if (!string.IsNullOrEmpty(state) && !string.IsNullOrEmpty(code_verifyer))
            {
                OauthAuthRequest authOptions = _configuration.GetSection("Oauthconfig").Get<OauthAuthRequest>();
                authOptions.State = _httpContextAccessor.HttpContext.Session.GetString("state");
                authOptions.CodeChallenge = GetCodeChallenge(_httpContextAccessor.HttpContext.Session.GetString("code_verifier"));
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
            tokenRequestOptions.code_verifier = _httpContextAccessor.HttpContext.Session.GetString("code_verifier");
            OauthTokenResponse response = await getTokenRequest(tokenRequestOptions);
            return response;
        }

        public void InitAuthRequest()
        {
            string code_verifier = GetRandomBase64String();
            SaveInSession("code_verifier", code_verifier);
            string state = GetRandomBase64String();
            SaveInSession("state", state);
        }

        public async Task SignIn(OauthTokenResponse? tokenResponse)
        {
            if (tokenResponse != null)
            {
                SaveInSession("access_token", tokenResponse.access_token);
                SaveInSession("refresh_token", tokenResponse.refresh_token);
                await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity()));
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

        private void SaveInSession(string key, string value)
        {
            _httpContextAccessor.HttpContext.Session.SetString(key, value);
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
