using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Models.Options;

namespace Assignment_Wt1_Oauth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetOauthAuthorizationUri()
        {
            return _configuration.GetSection("Oauthconfig").Get<OauthAuthorizationOptions>().ToString();
        }
    }
}
