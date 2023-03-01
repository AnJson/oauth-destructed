﻿using Assignment_Wt1_Oauth.Contracts;
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
            string authorizationUriWithOptions = _authService.GetOauthAuthorizationUri();
            return Redirect(authorizationUriWithOptions);
        }

        [Route("/session")]
        public IActionResult Session([FromQuery] string? code)
        {
            if (code == null)
            {
                return BadRequest();
            }

            try
            {
                OauthTokenResponse? tokenResponse = _authService.GetOauthToken(code);
                                                                            // Check what type of response if code is not correct!
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
