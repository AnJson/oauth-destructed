using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Controllers;
using Assignment_Wt1_Oauth.Models;
using Assignment_Wt1_Oauth.Utils;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;

namespace Assignment_Wt1_Oauth.Filters
{
    public class RefreshTokenActionFilter : IAsyncActionFilter
    {
        private readonly SessionHandler _sessionHandler;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public RefreshTokenActionFilter(SessionHandler sessionHandler, IConfiguration configuration, HttpClient httpClient)
        {
            _sessionHandler = sessionHandler;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is UserController userController)
            {
                int? tokenExpires = _sessionHandler.GetIntFromSession(SessionHandler.SessionStorageKey.TOKEN_EXPIRES);
                Int32 now = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                if (now < tokenExpires)
                {
                    OauthRefreshTokenRequest refreshTokenRequestOptions = _configuration.GetSection("Oauthconfig").Get<OauthRefreshTokenRequest>();
                    refreshTokenRequestOptions.code_verifier = context.HttpContext.Session.GetString(_sessionHandler.GetSessionStorageKey(SessionHandler.SessionStorageKey.CODE_VERIFIER));
                    refreshTokenRequestOptions.refresh_token = context.HttpContext.Session.GetString(_sessionHandler.GetSessionStorageKey(SessionHandler.SessionStorageKey.REFRESH_TOKEN));
                }

                await next();
            } else
            {
                await next();
            }
        }

        private async Task<OauthTokenResponse?> getRefreshTokenRequest(OauthRefreshTokenRequest options)
        {
            // Transform OauthRefreshTokenRequest to enumerable keyvalue pair.
            IEnumerable<KeyValuePair<string, string?>> optionsKeyValuePairs = options.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(string))
                .ToDictionary(p => p.Name, p => (string?)p.GetValue(options));

            FormUrlEncodedContent content = new FormUrlEncodedContent(optionsKeyValuePairs);
            string tokenUri = _configuration.GetValue<string>("Oauthconfig:token_uri");
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
