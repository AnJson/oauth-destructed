using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Filters;
using Assignment_Wt1_Oauth.Models;
using Assignment_Wt1_Oauth.Models.GroupsResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_Wt1_Oauth.Controllers
{
    /// <summary>
    /// Controller for user-routes, protected by authorization with automatic refresh-token handling.
    /// </summary>
    [Authorize]
    [TypeFilter(typeof(RefreshTokenActionFilter))]
    public class UserController : Controller
    {
        /// <summary>
        /// Injected userservice.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Constructor recieving injected services.
        /// </summary>
        /// <param name="userService">Service to inject.</param>
        public UserController(IUserService userService)
        { 
            _userService = userService;
        }

        /// <summary>
        /// Action-method for "/user".
        /// Returns bad request if not successfull fetching of profile.
        /// </summary>
        /// <returns>IActionResult</returns>
        [Route("[controller]")]
        public async Task<IActionResult> Index()
        {
            try
            {
                UserProfile? userProfile = await _userService.GetUserProfile();

                return View(userProfile);
            } catch (Exception e)
            {
                Console.WriteLine("An exception occurred: {0}", e.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// Action-method for "/user/activities".
        /// Returns bad request if not successfull fetching of activities.
        /// </summary>
        /// <returns>IActionResult</returns>
        [Route("[controller]/activities")]
        public async Task<IActionResult> Activities()
        {
            try
            {
                UserActivities userActivites = await _userService.GetActivities(101);
            
                return View(userActivites);
            } catch (Exception e)
            {
                Console.WriteLine("An exception occurred: {0}", e.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// Action-method for "/user/groups".
        /// Returns bad request if not soccessfull fetching of groupdata.
        /// </summary>
        /// <returns>IActionResult</returns>
        [Route("[controller]/groups")]
        public async Task<IActionResult> Groups()
        {
            try
            {
                GraphQLGroupsResponse? groupsViewData = await _userService.GetGroupCollection();
                return View(groupsViewData);
            } catch (Exception e)
            {
                Console.WriteLine("An exception occurred: {0}", e.Message);
                return BadRequest();
            }
        }
    }
}
