﻿using Assignment_Wt1_Oauth.Models.Options;

namespace Assignment_Wt1_Oauth.Contracts
{
    public interface IAuthService
    {
        public OauthAuthRequest GetOauthAuthorizationUri();
        public OauthTokenResponse GetOauthToken(string code);
    }
}
