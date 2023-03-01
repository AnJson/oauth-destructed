using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Filters;
using Assignment_Wt1_Oauth.Models.Options;
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
            OauthAuthRequest authRequestObject = _authService.GetOauthAuthorizationUri();
            HttpContext.Session.SetString("state", authRequestObject.State);
            return Redirect(authRequestObject.ToString());
        }

        /// <summary>
        /// Callback-route specified in gitlab oauth with filter to verfy that arguments are supplied.
        /// If arguments is not supplied a BadRequest-result is sent to the client.
        /// </summary>
        /// <param name="code">Code expected from gitlabs request, used in filter to verify that code is provided.</param>
        /// <param name="state">State expected from gitlabs request, used in filter to verify that state is provided.</param>
        /// <returns></returns>
        [Route("/session")]
        [TypeFilter(typeof(OauthCallbackActionFilter))]
        public IActionResult Session([FromQuery] string code, [FromQuery] string state)
        {
            // TODO: Move csrf-protection to service and call it here!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            try
            {
                // OauthTokenResponse? tokenResponse = _authService.GetOauthToken(code);
                                                                            // Check what type of response if code is not correct!
                ViewBag.code = code;
                return View(ViewBag);
            } catch (Exception e)
            {
                Console.WriteLine("An exception occurred: {0}", e.Message);
                return BadRequest();
            }

        }
    }
}
