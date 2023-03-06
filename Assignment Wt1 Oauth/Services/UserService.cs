using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Models;
using Assignment_Wt1_Oauth.Utils;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Assignment_Wt1_Oauth.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SessionHandler _sessionHandler;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public UserService(SessionHandler sessionHandler, IConfiguration configuration, HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _sessionHandler = sessionHandler;
            _configuration = configuration;
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GroupCollection> GetGroupCollection()
        {
            throw new NotImplementedException();
        }

        public async Task<UserProfile?> GetUserProfile()
        {
            return await RequestUserProfile();
        }

        private async Task<UserProfile?> RequestUserProfile()
        {
            string accessKey = _sessionHandler.GetFromSession(SessionHandler.SessionStorageKey.ACCESS_TOKEN);
            string profileUri = _configuration.GetValue<string>("Oauthconfig:ProfileUri");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessKey);
            HttpResponseMessage response = await _httpClient.GetAsync(profileUri);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"User get-request failed with statuscode {response.StatusCode}");
            }

            string responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserProfile>(responseContent);
        }
    }
}
