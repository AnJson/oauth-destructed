namespace Assignment_Wt1_Oauth.Models
{
    /// <summary>
    /// Wrapper for user data to extract from user response.
    /// </summary>
    public class UserProfile
    {
        public int? id { get; set; }
        public string? username { get; set; }
        public string? email { get; set; }
        public string? name { get; set; }
        public string? avatar_url { get; set; }
        public string? last_activity_on { get; set; }
    }
}
