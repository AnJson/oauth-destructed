﻿namespace Assignment_Wt1_Oauth.Models
{
    /// <summary>
    /// Model used to present activity in view.
    /// </summary>
    public class Activity
    {
        public string? action_name { get; set; }
        public string? target_type { get; set; }
        public string? target_title { get; set; }
        public DateTime? created_at { get; set; }

    }
}
