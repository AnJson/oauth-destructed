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
            string code_verifier = _authService.GetRandomBase64String();
            _authService.SaveInSession("code_verifier", code_verifier);
            Console.WriteLine($"Saved code_verifier: {code_verifier}");
            string state = _authService.GetRandomBase64String();
            _authService.SaveInSession("state", state);

            OauthAuthRequest authRequestObject = _authService.GetOauthAuthorizationUri(code_verifier);
            authRequestObject.State = state;
            return Redirect(authRequestObject.ToString());
        }

        /// <summary>
        /// Callback-route specified in gitlab oauth with filter to verfy that arguments are supplied.
        /// If arguments is not supplied a BadRequest-result is sent to the client.
        /// </summary>
        /// <param name="code">Expected from gitlabs request, used in filter to verify that code is provided.</param>
        /// <param name="state">Expected from gitlabs request, used in filter to verify that state is provided and csrf-verification.</param>
        /// <returns></returns>
        [Route("/session")]
        [TypeFilter(typeof(VerifyCallbackQueryParametersActionFilter))]
        [TypeFilter(typeof(OauthCsrfActionFilter))]
        public async Task<IActionResult> Session([FromQuery] string code, [FromQuery] string state)
        {
            try
            {
                OauthTokenResponse? tokenResponse = await _authService.GetOauthToken(code);
                // TODO: Sign user in!

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
