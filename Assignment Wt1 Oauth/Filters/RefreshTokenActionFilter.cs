using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Controllers;
using Assignment_Wt1_Oauth.Models;
using Assignment_Wt1_Oauth.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Assignment_Wt1_Oauth.Filters
{
    /// <summary>
    /// Refresh-token filter to handle the refresh of access token.
    /// </summary>
    public class RefreshTokenActionFilter : IAsyncActionFilter
    {
        /// <summary>
        /// Injected session-handler service.
        /// </summary>
        private readonly ISessionHandler _sessionHandler;
        
        /// <summary>
        /// Injected configuration service.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Injected request-handler service.
        /// </summary>
        private readonly IRequestHandler _requestHandler;

        /// <summary>
        /// Constructor recieving injected services.
        /// </summary>
        /// <param name="sessionHandler">Service to inject.</param>
        /// <param name="configuration">Service to inject.</param>
        /// <param name="requestHandler">Service to inject.</param>
        public RefreshTokenActionFilter(ISessionHandler sessionHandler, IConfiguration configuration, IRequestHandler requestHandler)
        {
            _sessionHandler = sessionHandler;
            _configuration = configuration;
            _requestHandler = requestHandler;
        }

        /// <summary>
        /// Method that is executed before action-method in controller is executed.
        /// Verifies expiration-time for access-token and uses refresh-token to request a new access-token if needed.
        /// Returns status-code 403 if not successfull refresh.
        /// </summary>
        /// <param name="context">The request-pipeline context.</param>
        /// <param name="next">The delegate function to proceed to next middleware.</param>
        /// <returns>Delegates to next middleware.</returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            
            if (context.Controller is UserController userController)
            {
                int? tokenExpires = _sessionHandler.GetIntFromSession(SessionHandler.SessionStorageKey.TOKEN_EXPIRES);
                Int32 now = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                if (!tokenExpires.HasValue)
                {
                    // No data in session storage.
                    await _sessionHandler.SignOut();
                    context.Result = userController.StatusCode(403);
                } else if (now > tokenExpires)
                {
                    // Access-token has expired.
                    try
                    {
                        // Requesting a new access-token.
                        OauthRefreshTokenRequest refreshTokenRequestOptions = _configuration.GetSection("Oauthconfig").Get<OauthRefreshTokenRequest>();
                        refreshTokenRequestOptions.codeVerifier = context.HttpContext.Session.GetString(_sessionHandler.GetSessionStorageKey(SessionHandler.SessionStorageKey.CODE_VERIFIER));
                        refreshTokenRequestOptions.refreshToken = context.HttpContext.Session.GetString(_sessionHandler.GetSessionStorageKey(SessionHandler.SessionStorageKey.REFRESH_TOKEN));
                        OauthTokenResponse tokenResponse = await _requestHandler.getTokenRequest(refreshTokenRequestOptions);

                        // Saving new data in session.
                        int expirationTime = (int)(tokenResponse.expires_in + tokenResponse.created_at);
                        _sessionHandler.SaveInSession(SessionHandler.SessionStorageKey.ACCESS_TOKEN, tokenResponse.access_token);
                        _sessionHandler.SaveInSession(SessionHandler.SessionStorageKey.REFRESH_TOKEN, tokenResponse.refresh_token);
                        _sessionHandler.SaveIntInSession(SessionHandler.SessionStorageKey.TOKEN_EXPIRES, expirationTime);

                        await next();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        await _sessionHandler.SignOut();
                        context.Result = userController.Forbid();
                    }
                }
                else
                {
                    await next();
                }
            } 
            else
            {
                await next();
            }
        }
    }
}
