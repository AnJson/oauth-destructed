using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_Wt1_Oauth.Controllers
{
    public class HomeController : Controller
    {
        private readonly IErrorService _errorService;

        public HomeController(IErrorService errorService)
        {
            _errorService = errorService;
        }

        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Error")]
        public IActionResult Error()
        {
            ViewBag.statusCode = _errorService.getErrorStatusCode();
            return View(ViewBag);
        }
    }
}
