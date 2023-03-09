using System.Diagnostics;

namespace Assignment_Wt1_Oauth.Models
{
    /// <summary>
    /// Wrapper for activities to be presented in view.
    /// </summary>
    public class UserActivities
    {
        public List<Activity> Activities { get; set; } = new List<Activity>();
    }
}
