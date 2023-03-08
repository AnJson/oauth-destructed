using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Filters;
using Assignment_Wt1_Oauth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_Wt1_Oauth.Controllers
{
    [AllowAnonymous]
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
            try
            {
                _authService.InitAuthRequest();
                OauthAuthRequest authRequestObject = _authService.GetOauthAuthorizationUri();
                return Redirect(authRequestObject.ToString());
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }

        [Authorize]
        [Route("/logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authService.SignOut();
                return Redirect("/");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Callback-route specified in gitlab oauth with filter to verfy that arguments are supplied.
        /// If arguments is not supplied a BadRequest-result is sent to the client.
        /// Saving tokens in session storage when successfully required.
        /// </summary>
        /// <param name="code">Expected from gitlabs request, used in filter to verify that code is provided.</param>
        /// <param name="state">Expected from gitlabs request, used in filter to verify that state is provided and csrf-verification.</param>
        /// <returns>Redirect to "/user"</returns>
        [Route("/session")]
        [TypeFilter(typeof(VerifyCallbackQueryParametersActionFilter))]
        [TypeFilter(typeof(OauthCsrfActionFilter))]
        public async Task<IActionResult> Session([FromQuery] string code, [FromQuery] string state)
        {
            try
            {
                OauthTokenResponse? tokenResponse = await _authService.GetOauthToken(code);

                await _authService.SignIn(tokenResponse);

                return Redirect("/user");
            } catch (Exception e)
            {
                Console.WriteLine("An exception occurred: {0}", e.Message);
                return BadRequest();
            }
        }

        [Route("/denied")]
        public IActionResult AccessDenied()
        {
            return Forbid();
        }
    }
}
