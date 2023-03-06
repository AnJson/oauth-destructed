namespace Assignment_Wt1_Oauth.Models.GroupsResponse
{
    public class Group
    {
        public string name { get; set; }
        public string webUrl { get; set; }
        public object avatarUrl { get; set; }
        public string fullPath { get; set; }
        public Projects projects { get; set; }
    }


}
