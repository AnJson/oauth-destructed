using System.Diagnostics;

namespace Assignment_Wt1_Oauth.Models
{
    public class UserActivities
    {
        public List<Activity> Activities { get; set; } = new List<Activity>();

        public void AddActivity(Activity activity)
        {
            Activities.Add(activity);
        }
    }
}
