using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Controllers;
using Assignment_Wt1_Oauth.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Assignment_Wt1_Oauth.Filters
{
    public class OauthCsrfActionFilter : IAsyncActionFilter
    {
        private readonly SessionHandler _sessionHandler;
        private readonly IConfiguration _configuration;

        public OauthCsrfActionFilter(SessionHandler sessionHandler, IConfiguration configuration)
        {
            _sessionHandler = sessionHandler;
            _configuration = configuration;
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
                    context.HttpContext.Response.Cookies.Delete(_configuration.GetValue<string>("session_cookie"));
                    await context.HttpContext.SignOutAsync();
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
