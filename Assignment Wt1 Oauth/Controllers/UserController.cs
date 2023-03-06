using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_Wt1_Oauth.Controllers
{
    [Authorize]
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
        public IActionResult Activities(string id)
        {
            return View();
        }

        [Route("[controller]/groups")]
        public async Task<IActionResult> Groups(string id)
        {
            GroupCollection groupsViewData = await _userService.GetGroupCollection();
            return View(groupsViewData);
        }
    }
}
