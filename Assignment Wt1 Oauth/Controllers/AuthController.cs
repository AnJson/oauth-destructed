using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Filters;
using Assignment_Wt1_Oauth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_Wt1_Oauth.Controllers
{
    /// <summary>
    /// Controller that handles authentication.
    /// </summary>
    [AllowAnonymous]
    public class AuthController : Controller
    {
        /// <summary>
        /// Injected authservice.
        /// </summary>
        private readonly IAuthService _authService;

        /// <summary>
        /// Constructor recieving injected services.
        /// </summary>
        /// <param name="authService">Service to inject.</param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Action-method for "/login".
        /// Redirects to Gitlabs authentication endpoint.
        /// Returns status-code 500 if not successfull construction of auth-uri.
        /// </summary>
        /// <returns>IActionResult</returns>
        [Route("/login")]
        public IActionResult Login()
        {
            try
            {
                OauthAuthRequest authRequestObject = _authService.GetOauthAuthorizationUri();
                return Redirect(authRequestObject.ToString());
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Action-method for "/logout".
        /// Removes session-data and deletes cookies.
        /// Returns status-code 500 if error in signout.
        /// </summary>
        /// <returns>IActionResult</returns>
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
        /// Callback-route specified in gitlab oauth with filter to verfy that arguments are supplied and with csrf-check.
        /// Redirects to "/user".
        /// If arguments is not supplied a BadRequest-result is sent to the client.
        /// Saving tokens in session storage when successfully required.
        /// </summary>
        /// <param name="code">Expected from gitlabs request, used in filter to verify that code is provided.</param>
        /// <param name="state">Expected from gitlabs request, used in filter to verify that state is provided and csrf-verification.</param>
        /// <returns>IActionResult</returns>
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

        /// <summary>
        /// Action-method for "/denied".
        /// </summary>
        /// <returns>IActionResult with statuscode 403.</returns>
        [Route("/denied")]
        public IActionResult AccessDenied()
        {
            return Forbid();
        }
    }
}
