namespace Assignment_Wt1_Oauth.Models
{
    public class Group
    {
        public string? name { get; set; }
        public string? webUrl { get; set; }
        public string? avatarUrl { get; set; }
        public string? fullPath { get; set; }
        public List<ProjectCollection>? projects { get; set; }
    }
}
