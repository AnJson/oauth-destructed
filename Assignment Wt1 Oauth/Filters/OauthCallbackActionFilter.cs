﻿using Assignment_Wt1_Oauth.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Assignment_Wt1_Oauth.Filters
{
    public class OauthCallbackActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is AuthController authController)
            {
                if (!context.ActionArguments.ContainsKey("code"))
                {
                    context.Result = authController.BadRequest();
                }

                if (!context.ActionArguments.ContainsKey("state"))
                {
                    context.Result = authController.BadRequest();
                }

                if (string.IsNullOrEmpty(Convert.ToString(!context.ActionArguments.ContainsKey("code"))))
                {
                    context.Result = authController.BadRequest();
                }

                string state = Convert.ToString(!context.ActionArguments.ContainsKey("state"));

                if (state != context.HttpContext.Session.GetString("state"))
                {
                    context.HttpContext.Session.Clear();
                    context.Result = authController.Redirect("/");
                }

                await next();
            } else
            {
                await next();
            }

        }
    }
}
