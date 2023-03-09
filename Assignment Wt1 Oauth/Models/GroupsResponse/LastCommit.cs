namespace Assignment_Wt1_Oauth.Models.GroupsResponse
{
    /// <summary>
    /// Model for last_commit property in GraphQL-response.
    /// </summary>
    public class LastCommit
    {
        public DateTime authoredDate { get; set; }
        public Author author { get; set; }
    }


}
