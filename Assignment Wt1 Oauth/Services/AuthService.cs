using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Models.Options;
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

        public OauthAuthRequest GetOauthAuthorizationUri(string code_verifier)
        {
            OauthAuthRequest authOptions = _configuration.GetSection("Oauthconfig").Get<OauthAuthRequest>();
            authOptions.CodeChallenge = GetCodeChallenge(code_verifier);
            return authOptions;
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

        public string GetCodeChallenge(string text)
        {
            byte[] sha256Bytes = SHA256.HashData(Encoding.UTF8.GetBytes(text));
            string base64String = Convert.ToBase64String(sha256Bytes);
            return base64String.Replace('+', '-').Replace('/', '_').TrimEnd('=');
        }

        public string GetRandomBase64String()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString())).Replace("=", "y").Replace("+", "-").Replace("/", "_");
        }

        public void SaveInSession(string key, string code_verifier)
        {
            _httpContextAccessor.HttpContext.Session.SetString(key, code_verifier);
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
