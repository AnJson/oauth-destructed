using Microsoft.AspNetCore.Mvc;

namespace Assignment_Wt1_Oauth.Controllers
{
    public class UserController : Controller
    {
        [Route("[controller]/{id}")]
        public IActionResult Index(string id)
        {
            ViewBag.userid = id;
            return View(ViewBag);
        }

        [Route("[controller]/{id}/activities")]
        public IActionResult Activities(string id)
        {
            return View();
        }

        [Route("[controller]/{id}/groups")]
        public IActionResult Groups(string id)
        {
            return View();
        }
    }
}
