using Assignment_Wt1_Oauth.Models.Options;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_Wt1_Oauth.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("/login")]
        public IActionResult Login()
        {
            OauthAuthorizationOptions authorizationUriWithOptions = _configuration.GetSection("Oauthconfig").Get<OauthAuthorizationOptions>();
            return Redirect(authorizationUriWithOptions.ToString());
        }

        [EnableCors("Gitlab")]
        [Route("/session")]
        public IActionResult Session()
        {
            // TODO: Fix this problem from gitlab.
            if (!HttpContext.Request.Query.ContainsKey("code")) // Always contains code???
            {
                ViewBag.code = "No code in querytring";
            }

            ViewBag.code = HttpContext.Request.Query["code"];

            return View(ViewBag);
        }
    }
}
