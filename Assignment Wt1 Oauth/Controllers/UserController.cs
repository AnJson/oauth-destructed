using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Filters;
using Assignment_Wt1_Oauth.Models;
using Assignment_Wt1_Oauth.Models.GroupsResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_Wt1_Oauth.Controllers
{
    [Authorize]
    // [TypeFilter(typeof(RefreshTokenActionFilter))]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        { 
            _userService = userService;
        }

        [Route("[controller]")]
        public async Task<IActionResult> Index()
        {
            UserProfile? userProfile = await _userService.GetUserProfile();

            return View(userProfile);
        }

        [Route("[controller]/activities")]
        public async Task<IActionResult> Activities(string id)
        {
            UserActivities userActivites = await _userService.GetActivities(101);
            
            return View(userActivites);
        }

        [Route("[controller]/groups")]
        public async Task<IActionResult> Groups()
        {
            GraphQLGroupsResponse? groupsViewData = await _userService.GetGroupCollection();
            return View(groupsViewData);
        }
    }
}
