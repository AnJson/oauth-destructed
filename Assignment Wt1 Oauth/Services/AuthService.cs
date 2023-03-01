using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Models.Options;
using System.Security.Cryptography;
using System.Text;

namespace Assignment_Wt1_Oauth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public AuthService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

        }

        public OauthAuthRequest GetOauthAuthorizationUri(string code_verifier)
        {
            OauthAuthRequest authOptions = _configuration.GetSection("Oauthconfig").Get<OauthAuthRequest>();
            authOptions.CodeChallenge = GetCodeChallenge(code_verifier);
            return authOptions;
        }

        public OauthTokenResponse GetOauthToken(string code)
        {
            throw new NotImplementedException();
        }

        public string GetCodeChallenge(string text)
        {
            byte[] sha256Bytes = SHA256.HashData(Encoding.UTF8.GetBytes(text));
            string base64String = Convert.ToBase64String(sha256Bytes);
            return base64String.Replace('+', '-').Replace('/', '_').TrimEnd('=');
        }

        public string GetRandomBase64String()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString())).Replace("=", "").Replace("+", "-").Replace("/", "_");
        }

        public void SaveInSession(string key, string code_verifier)
        {
            _httpContextAccessor.HttpContext.Session.SetString(key, code_verifier);
        }
    }
}
