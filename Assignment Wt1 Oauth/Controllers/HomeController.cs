using Assignment_Wt1_Oauth.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_Wt1_Oauth.Controllers
{
    /// <summary>
    /// Controller for home-route and error-route.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Injected errorservice.
        /// </summary>
        private readonly IErrorService _errorService;

        /// <summary>
        /// Constructor recieving injected services.
        /// </summary>
        /// <param name="errorService">Service to inject.</param>
        public HomeController(IErrorService errorService)
        {
            _errorService = errorService;
        }

        /// <summary>
        /// Action-method for "/".
        /// </summary>
        /// <returns>IActionResult</returns>
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Action-method for "/error".
        /// </summary>
        /// <param name="statusCode">Statuscode to display in view.</param>
        /// <returns></returns>
        [Route("/Error")]
        public IActionResult Error([FromQuery] string? statusCode)
        {
            if (statusCode == null)
            {
                ViewBag.statusCode = _errorService.getErrorStatusCode();
            } else
            {
                ViewBag.statusCode = statusCode;
            }

            return View(ViewBag);
        }
    }
}
