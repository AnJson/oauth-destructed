using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Models;
using Assignment_Wt1_Oauth.Utils;
using System.Security.Cryptography;
using System.Text;

namespace Assignment_Wt1_Oauth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ISessionHandler _sessionHandler;
        private readonly IRequestHandler _requestHandler;

        public AuthService(IConfiguration configuration, ISessionHandler sessionHandler, IRequestHandler requestHandler)
        {
            _configuration = configuration;
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
            await _sessionHandler.SignIn(tokenResponse);
        }

        public async Task SignOut()
        {
            await _sessionHandler.SignOut();
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
