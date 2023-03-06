using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Models;
using Assignment_Wt1_Oauth.Models.GraphQLGroupsResponse;
using Assignment_Wt1_Oauth.Utils;
using GraphQL;
using GraphQL.Client.Abstractions;
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
        private readonly IGraphQLClient _graphqlClient;

        public UserService(SessionHandler sessionHandler, IConfiguration configuration, IGraphQLClient graphqlClient, HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _sessionHandler = sessionHandler;
            _configuration = configuration;
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _graphqlClient = graphqlClient;
        }

        public async Task<GroupCollection> GetGroupCollection()
        {
            var graphqlResponse = await RequestGroups();
            return null;
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

        private async Task<string?> RequestGroups()
        {
            string accessKey = _sessionHandler.GetFromSession(SessionHandler.SessionStorageKey.ACCESS_TOKEN);
            // string graphqlUri = _configuration.GetValue<string>("Oauthconfig:GraphqlUri");

            GraphQLRequest request = new GraphQLRequest
            {
                Query = @"query {
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

            var response = await _graphqlClient.SendQueryAsync(request); // NOT WORKING!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            return null;
            // _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessKey);
            // HttpResponseMessage response = await _httpClient.PostAsync(graphqlUri); // Add body.

            // if (!response.IsSuccessStatusCode)
            // {
            //    throw new Exception($"Groups graphql request failed with statuscode {response.StatusCode}");
            // }

            // return await response.Content.ReadAsStringAsync();
            // return JsonSerializer.Deserialize<UserProfile>(responseContent);
        }
    }
}
