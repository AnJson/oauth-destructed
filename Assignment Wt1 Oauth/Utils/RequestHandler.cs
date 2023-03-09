using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Models;
using Assignment_Wt1_Oauth.Models.GroupsResponse;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Assignment_Wt1_Oauth.Utils
{
    /// <summary>
    /// Handles sending http-requests.
    /// </summary>
    public class RequestHandler : IRequestHandler
    {
        /// <summary>
        /// Injected service.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Injected service.
        /// </summary>
        private readonly ISessionHandler _sessionHandler;

        /// <summary>
        /// Injected service.
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructor recieving injected services.
        /// </summary>
        /// <param name="configuration">Service to inject.</param>
        /// <param name="httpClient">Service to inject.</param>
        /// <param name="sessionHandler">Service to inject.</param>
        public RequestHandler (IConfiguration configuration, HttpClient httpClient, ISessionHandler sessionHandler)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _sessionHandler = sessionHandler;
        }

        /// <summary>
        /// Constructs and sends request to fetch oauth token.
        /// </summary>
        /// <param name="options">Wrapper model for the token request options.</param>
        /// <returns>Wrapper model for the token-response.</returns>
        /// <exception cref="Exception">If request is not successfull.</exception>
        public async Task<OauthTokenResponse?> getTokenRequest(OauthTokenRequest options)
        {
            // Transform OauthTokenRequest to enumerable keyvaluepair.
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

        /// <summary>
        /// Constructs and sends request to refresh oauth token.
        /// </summary>
        /// <param name="options">Wrapper model for the token request options.</param>
        /// <returns>Wrapper model for the token-response.</returns>
        /// <exception cref="Exception">If request is not successfull.</exception>
        public async Task<OauthTokenResponse?> getTokenRequest(OauthRefreshTokenRequest options)
        {
            // Transform OauthTokenRequest to enumerable keyvaluepair.
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

        /// <summary>
        /// Constructs and sends request for user profile.
        /// </summary>
        /// <returns>Wrapper model for profile-response.</returns>
        /// <exception cref="Exception">If not successful request.</exception>
        public async Task<UserProfile?> getUserProfile()
        {
            string accessKey = _sessionHandler.GetFromSession(SessionHandler.SessionStorageKey.ACCESS_TOKEN);
            string profileUri = _configuration.GetValue<string>("Oauthconfig:profile_uri");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessKey);
            HttpResponseMessage response = await _httpClient.GetAsync(profileUri);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"User get-request failed with statuscode {response.StatusCode}");
            }

            string responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserProfile>(responseContent);
        }

        /// <summary>
        /// Constructs and sends GraphQL request for groups and projects.
        /// </summary>
        /// <returns>Wrapper model for the GraphQL response.</returns>
        /// <exception cref="Exception">If not successful request.</exception>
        public async Task<GraphQLGroupsResponse?> getGroups()
        {
            string accessKey = _sessionHandler.GetFromSession(SessionHandler.SessionStorageKey.ACCESS_TOKEN);
            string graphqlUri = _configuration.GetValue<string>("GraphqlConfig:graphql_uri");

            var queryObject = new
            {
                query = _configuration.GetValue<string>("GraphqlConfig:groups_query")
            };

            var query = new StringContent(JsonSerializer.Serialize(queryObject), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessKey);
            HttpResponseMessage response = await _httpClient.PostAsync(graphqlUri, query);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Groups graphql request failed with statuscode {response.StatusCode}");
            }

            string responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<GraphQLGroupsResponse>(responseContent);
        }

        /// <summary>
        /// Constructs and sends the number of requests needed to fetch requested number of activities.
        /// </summary>
        /// <param name="requestedActivities">Number of activities to fetch.</param>
        /// <returns>Wrapper model for the activities response.</returns>
        /// <exception cref="Exception">If not successful request.</exception>
        public async Task<UserActivities> getActivites(int requestedActivities)
        {
            string accessKey = _sessionHandler.GetFromSession(SessionHandler.SessionStorageKey.ACCESS_TOKEN);
            ActivitiesRequest activitiesRequest = _configuration.GetSection("OauthConfig").Get<ActivitiesRequest>();
            activitiesRequest.page = 1;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessKey);

            List <Activity> activities = new List<Activity>();
            while (activities.Count < requestedActivities)
            {
                // Set the pre_page query value to max or to requested amount of activities if lower than max.
                activitiesRequest.per_page = Math.Min(100, requestedActivities);

                HttpResponseMessage response = await _httpClient.GetAsync(activitiesRequest.ToString());

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Groups activites request failed with statuscode {response.StatusCode}");
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                List<Activity> fetchedActivities = JsonSerializer.Deserialize<List<Activity>>(responseContent);

                // May be overfetching and to only return a list of max 101 activities, less than the fetched amount of activities may have to be added.
                int activitiesToAdd = Math.Min(100, (requestedActivities - activities.Count));
                activities.AddRange(fetchedActivities.Take(activitiesToAdd));
                activitiesRequest.page++;
            }

            return new UserActivities
            {
                Activities = activities
            };
        }
    }
}
