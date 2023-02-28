using Assignment_Wt1_Oauth.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_Wt1_Oauth.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Route("/login")]
        public IActionResult Login()
        {
            string authorizationUriWithOptions = _authService.GetOauthAuthorizationUri();
            return Redirect(authorizationUriWithOptions);
        }

        [Route("/session")]
        public IActionResult Session([FromQuery] string? code)
        {
            if (code == null)
            {
                return BadRequest(400);
            }

            ViewBag.code = code;
            return View(ViewBag);
        }
    }
}
