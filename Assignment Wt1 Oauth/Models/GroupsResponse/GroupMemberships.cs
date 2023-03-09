namespace Assignment_Wt1_Oauth.Models.GroupsResponse
{
    /// <summary>
    /// Model for group_memberships property in GraphQL-response.
    /// </summary>
    public class GroupMemberships
    {
        public PageInfo pageInfo { get; set; }
        public List<Node> nodes { get; set; }
    }


}
