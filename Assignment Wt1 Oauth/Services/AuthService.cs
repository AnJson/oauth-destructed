using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Models;
using Assignment_Wt1_Oauth.Utils;
using System.Security.Cryptography;
using System.Text;

namespace Assignment_Wt1_Oauth.Services
{
    /// <summary>
    /// Service that handles authentication.
    /// </summary>
    public class AuthService : IAuthService
    {
        /// <summary>
        /// Injected configuration service.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Injected session-handler service.
        /// </summary>
        private readonly ISessionHandler _sessionHandler;

        /// <summary>
        /// Injected request-handler service.
        /// </summary>
        private readonly IRequestHandler _requestHandler;

        /// <summary>
        /// Constructor that recieves injected services.
        /// </summary>
        /// <param name="configuration">Injected service.</param>
        /// <param name="sessionHandler">Injected service.</param>
        /// <param name="requestHandler">Injected service.</param>
        public AuthService(IConfiguration configuration, ISessionHandler sessionHandler, IRequestHandler requestHandler)
        {
            _configuration = configuration;
            _sessionHandler = sessionHandler;
            _requestHandler = requestHandler;
        }

        /// <summary>
        /// Saves code_verifier and state in session to be used in token-request and csrf-filter later.
        /// </summary>
        /// <returns>Wrapper model for the oauth auth-request.</returns>
        public OauthAuthRequest GetOauthAuthorizationUri()
        {
            InitAuthRequest();
            string state = _sessionHandler.GetFromSession(SessionHandler.SessionStorageKey.STATE);
            string code_verifyer = _sessionHandler.GetFromSession(SessionHandler.SessionStorageKey.CODE_VERIFIER);
  
            OauthAuthRequest authOptions = _configuration.GetSection("Oauthconfig").Get<OauthAuthRequest>();
            authOptions.state = state;
            authOptions.codeChallenge = GetCodeChallenge(code_verifyer);
            return authOptions;
        }

        /// <summary>
        /// Constructs query options and sends oauth token-request.
        /// </summary>
        /// <param name="code">Recieved code from the oauth auth-request.</param>
        /// <returns>Wrapper model for token response.</returns>
        public async Task<OauthTokenResponse?> GetOauthToken(string code)
        {
            OauthTokenRequest tokenRequestOptions = _configuration.GetSection("Oauthconfig").Get<OauthTokenRequest>();
            tokenRequestOptions.code = code;
            tokenRequestOptions.codeVerifier = _sessionHandler.GetFromSession(SessionHandler.SessionStorageKey.CODE_VERIFIER);
            OauthTokenResponse response = await _requestHandler.getTokenRequest(tokenRequestOptions);
            return response;
        }

        /// <summary>
        /// Creating auth-cookie and saving token-data to session.
        /// </summary>
        /// <param name="tokenResponse">The token-response from gitlab oauth, used to identify user in auth-cookie.</param>
        /// <returns></returns>
        public async Task SignIn(OauthTokenResponse? tokenResponse)
        {
            await _sessionHandler.SignIn(tokenResponse);
        }

        /// <summary>
        /// Cleaning up session and deletes cookies.
        /// </summary>
        /// <returns></returns>
        public async Task SignOut()
        {
            await _sessionHandler.SignOut();
        }

        /// <summary>
        /// Generates a valid code_challenge string used in oauth authentication.
        /// </summary>
        /// <param name="text">The code_verifier used in the oauth authentication.</param>
        /// <returns>A valid code_challenge.</returns>
        private string GetCodeChallenge(string text)
        {
            byte[] sha256Bytes = SHA256.HashData(Encoding.UTF8.GetBytes(text));
            string base64String = Convert.ToBase64String(sha256Bytes);
            return base64String.Replace('+', '-').Replace('/', '_').TrimEnd('=');
        }

        /// <summary>
        /// Generates a valid code_verifier string used in the oauth authentication.
        /// </summary>
        /// <returns>A valid code_verifier</returns>
        private string GetRandomBase64String()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString())).Replace("=", "y").Replace("+", "-").Replace("/", "_");
        }

        /// <summary>
        /// Generates and saves code_challenge and code_verifier in session.
        /// </summary>
        private void InitAuthRequest()
        {
            string code_verifier = GetRandomBase64String();
            _sessionHandler.SaveInSession(SessionHandler.SessionStorageKey.CODE_VERIFIER, code_verifier);
            string state = GetRandomBase64String();
            _sessionHandler.SaveInSession(SessionHandler.SessionStorageKey.STATE, state);
        }
    }
}
