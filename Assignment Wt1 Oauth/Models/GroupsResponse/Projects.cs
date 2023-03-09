namespace Assignment_Wt1_Oauth.Models.GroupsResponse
{
    /// <summary>
    /// Model for projects property in GraphQL-response.
    /// </summary>
    public class Projects
    {
        public PageInfo pageInfo { get; set; }
        public List<Node> nodes { get; set; }
    }


}
