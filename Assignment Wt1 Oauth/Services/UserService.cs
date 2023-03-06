using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Models;
using Assignment_Wt1_Oauth.Models.GroupsResponse;
using Assignment_Wt1_Oauth.Utils;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Assignment_Wt1_Oauth.Services
{
    public class UserService : IUserService
    {
        private readonly SessionHandler _sessionHandler;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public UserService(SessionHandler sessionHandler, IConfiguration configuration, HttpClient httpClient)
        {
            _sessionHandler = sessionHandler;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<GraphQLGroupsResponse?> GetGroupCollection()
        {
            return await RequestGroups();
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

        private async Task<GraphQLGroupsResponse?> RequestGroups()
        {
            string accessKey = _sessionHandler.GetFromSession(SessionHandler.SessionStorageKey.ACCESS_TOKEN);
            string graphqlUri = _configuration.GetValue<string>("Oauthconfig:GraphqlUri");

            var queryObject = new
            {
                query = @"query {
                          currentUser {
                            groupMemberships(first: 3) {
                              pageInfo {
                                hasNextPage
                              }
                              nodes {
                                group {
                                  name
                                  webUrl
                                  avatarUrl
                                  fullPath
                                  projects(first: 5, includeSubgroups: true) {
                                    pageInfo {
                                      hasNextPage
                                    }
                                    nodes {
                                      name
                                      webUrl
                                      avatarUrl
                                      fullPath
                                      nameWithNamespace
                                      lastActivityAt #date of last commit or push event
                                      repository {
                                        tree {
                                          lastCommit {
                                            authoredDate
                                            author {
                                              name
                                              avatarUrl
                                              username
                                            }
                                          }
                                        }
                                      }
                                    }
                                  }
                                }
                              }
                            }
                          }
                        }"
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
    }
}
