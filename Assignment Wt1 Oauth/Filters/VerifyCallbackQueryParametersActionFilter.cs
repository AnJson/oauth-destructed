using Assignment_Wt1_Oauth.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Assignment_Wt1_Oauth.Filters
{
    /// <summary>
    /// Filter to verify url query-parameters.
    /// </summary>
    public class VerifyCallbackQueryParametersActionFilter : IAsyncActionFilter
    {
        /// <summary>
        /// Method that is executed before action-method in controller is executed.
        /// Verifies that "code" and "state" are in url query-parameters and that they are not null.
        /// Returns bad request if not valid query.
        /// </summary>
        /// <param name="context">The request-pipeline context.</param>
        /// <param name="next">The delegate function to proceed to next middleware.</param>
        /// <returns>Delegates to next middleware.</returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is AuthController authController)
            {
                if (!isArgumentInActionArguments("code", context) ||
                    isArgumentNullOrEmpty("code", context) ||
                    !isArgumentInActionArguments("state", context) ||
                    isArgumentNullOrEmpty("state", context))
                {
                    context.Result = authController.BadRequest();
                } else
                {
                    await next();
                }
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
