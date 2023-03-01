using Assignment_Wt1_Oauth.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Assignment_Wt1_Oauth.Filters
{
    public class VerifyCallbackQueryParametersActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is AuthController authController)
            {
                // TODO: FIX THIS!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                if (!isArgumentInActionArguments("code", context) ||
                    !isArgumentInActionArguments("state", context) ||
                    isArgumentNullOrEmpty("code", context) ||
                    isArgumentNullOrEmpty("state", context))
                {
                    context.Result = authController.BadRequest();
                } else
                {
                    await next();
                }
                /* // TODO Move to service!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                string state = Convert.ToString(context.ActionArguments["state"]);

                if (state != context.HttpContext.Session.GetString("state"))
                {
                    context.HttpContext.Session.Clear();
                    context.Result = authController.Redirect("/");
                }
                */
            } else
            {
                await next();
            }
        }

        private Boolean isArgumentNullOrEmpty(string argument, ActionExecutingContext context)
        {
            return string.IsNullOrEmpty(Convert.ToString(context.ActionArguments[argument]));
        }

        private Boolean isArgumentInActionArguments(string argument, ActionExecutingContext context)
        {
            return context.ActionArguments.ContainsKey(argument);
        }
    }
}
