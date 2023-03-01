using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Models.Options;
using System.Security.Cryptography;
using System.Text;

namespace Assignment_Wt1_Oauth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public OauthAuthRequest GetOauthAuthorizationUri()
        {
            OauthAuthRequest authOptions = _configuration.GetSection("Oauthconfig").Get<OauthAuthRequest>();
            authOptions.State = getState();
            authOptions.CodeChallenge = getCodeChallenge();
            return authOptions;
        }

        public OauthTokenResponse GetOauthToken(string code)
        {
            throw new NotImplementedException();
        }

        private string getCodeChallenge()
        {
            string code_verifier = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString())).Replace("=", "").Replace("+", "-").Replace("/", "_");
            byte[] sha256Bytes = SHA256.HashData(Encoding.UTF8.GetBytes(code_verifier));
            string base64String = Convert.ToBase64String(sha256Bytes);
            return base64String.Replace('+', '-').Replace('/', '_').TrimEnd('=');
        }

        private string getState()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
