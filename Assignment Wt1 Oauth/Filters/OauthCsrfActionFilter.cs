using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Controllers;
using Assignment_Wt1_Oauth.Utils;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Assignment_Wt1_Oauth.Filters
{
    /// <summary>
    /// CSRF-filter for oauth authentication.
    /// </summary>
    public class OauthCsrfActionFilter : IAsyncActionFilter
    {
        /// <summary>
        /// Injected service.
        /// </summary>
        private readonly ISessionHandler _sessionHandler;

        /// <summary>
        /// Constructor recieving injected service.
        /// </summary>
        /// <param name="sessionHandler">Service to inject.</param>
        public OauthCsrfActionFilter(ISessionHandler sessionHandler)
        {
            _sessionHandler = sessionHandler;
        }

        /// <summary>
        /// Method that is executed before action-method in controller is executed.
        /// Verifies that the recieved state has not changed since the auth-request was sent.
        /// Redirects to "/" if not valid state.
        /// </summary>
        /// <param name="context">The request-pipeline context.</param>
        /// <param name="next">The delegate function to proceed to next middleware.</param>
        /// <returns>Delegates to next middleware.</returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is AuthController authController)
            {
                string recievedState = Convert.ToString(context.ActionArguments["state"]);
                string originalState = _sessionHandler.GetFromSession(SessionHandler.SessionStorageKey.STATE);

                if (recievedState != originalState)
                {
                    await _sessionHandler.SignOut();
                    context.Result = authController.Redirect("/");
                } else
                {
                    await next();
                }
            } else
            {
                await next();
            }
        }
    }
}
