namespace Assignment_Wt1_Oauth.Models
{
    public class Project
    {
        public string? name { get; set; }
        public string? webUrl { get; set; }
        public string? avatarUrl { get; set; }
        public string? fullPath { get; set; }
        public string? nameWithNamespace { get; set; }
        public string? lastActivityAt { get; set; }
        public Commit? lastCommit { get; set; }
    }
}
