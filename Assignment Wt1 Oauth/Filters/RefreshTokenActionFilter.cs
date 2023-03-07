﻿using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Controllers;
using Assignment_Wt1_Oauth.Models;
using Assignment_Wt1_Oauth.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;

namespace Assignment_Wt1_Oauth.Filters
{
    public class RefreshTokenActionFilter : IAsyncActionFilter
    {
        private readonly SessionHandler _sessionHandler;
        private readonly IConfiguration _configuration;
        private readonly IRequestHandler _requestHandler;

        public RefreshTokenActionFilter(SessionHandler sessionHandler, IConfiguration configuration, IRequestHandler requestHandler)
        {
            _sessionHandler = sessionHandler;
            _configuration = configuration;
            _requestHandler = requestHandler;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is UserController userController)
            {
                int? tokenExpires = _sessionHandler.GetIntFromSession(SessionHandler.SessionStorageKey.TOKEN_EXPIRES);
                Int32 now = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                if (now > tokenExpires)
                {
                    try
                    {
                        OauthRefreshTokenRequest refreshTokenRequestOptions = _configuration.GetSection("Oauthconfig").Get<OauthRefreshTokenRequest>();
                        refreshTokenRequestOptions.code_verifier = context.HttpContext.Session.GetString(_sessionHandler.GetSessionStorageKey(SessionHandler.SessionStorageKey.CODE_VERIFIER));
                        refreshTokenRequestOptions.refresh_token = context.HttpContext.Session.GetString(_sessionHandler.GetSessionStorageKey(SessionHandler.SessionStorageKey.REFRESH_TOKEN));
                        OauthTokenResponse tokenResponse = await _requestHandler.getTokenRequest(refreshTokenRequestOptions);

                        int expirationTime = (int)(tokenResponse.expires_in + tokenResponse.created_at);
                        _sessionHandler.SaveInSession(SessionHandler.SessionStorageKey.ACCESS_TOKEN, tokenResponse.access_token);
                        _sessionHandler.SaveInSession(SessionHandler.SessionStorageKey.REFRESH_TOKEN, tokenResponse.refresh_token);
                        _sessionHandler.SaveIntInSession(SessionHandler.SessionStorageKey.TOKEN_EXPIRES, expirationTime);

                        await next();
                    } catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        context.HttpContext.Session.Clear();
                        context.HttpContext.Response.Cookies.Delete(_configuration.GetValue<string>("session_cookie"));
                        await context.HttpContext.SignOutAsync();
                        context.Result = userController.Forbid();
                    }
                }

            } else
            {
                await next();
            }
        }
    }
}
