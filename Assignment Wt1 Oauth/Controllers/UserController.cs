using Microsoft.AspNetCore.Mvc;

namespace Assignment_Wt1_Oauth.Controllers
{
    public class UserController : Controller
    {
        [Route("[controller]")]
        public IActionResult Index(string id)
        {
            ViewBag.userid = id;
            return View(ViewBag);
        }

        [Route("[controller]/activities")]
        public IActionResult Activities(string id)
        {
            return View();
        }

        [Route("[controller]/groups")]
        public IActionResult Groups(string id)
        {
            return View();
        }
    }
}
