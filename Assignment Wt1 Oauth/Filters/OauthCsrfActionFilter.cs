using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Controllers;
using Assignment_Wt1_Oauth.Utils;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Assignment_Wt1_Oauth.Filters
{
    public class OauthCsrfActionFilter : IAsyncActionFilter
    {
        private readonly SessionHandler _sessionHandler;

        public OauthCsrfActionFilter(SessionHandler sessionHandler)
        {
            _sessionHandler = sessionHandler;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is AuthController authController)
            {
                string recievedState = Convert.ToString(context.ActionArguments["state"]);
                string originalState = _sessionHandler.GetFromSession(SessionHandler.SessionStorageKey.STATE);

                if (recievedState != originalState)
                {
                    context.HttpContext.Session.Clear();
                    context.HttpContext.Response.Cookies.Delete(".AspNetCore.Session");
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
