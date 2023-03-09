namespace Assignment_Wt1_Oauth.Models.GroupsResponse
{
    /// <summary>
    /// Model for node property in GraphQL-response.
    /// </summary>
    public class Node
    {
        public Group group { get; set; }
        public string name { get; set; }
        public string webUrl { get; set; }
        public string avatarUrl { get; set; }
        public string fullPath { get; set; }
        public string nameWithNamespace { get; set; }
        public DateTime lastActivityAt { get; set; }
        public Repository repository { get; set; }
    }


}
