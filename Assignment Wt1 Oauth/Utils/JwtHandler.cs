using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Assignment_Wt1_Oauth.Utils
{
    public class JwtHandler : IJwtHandler
    {
        public IdTokenData GetIdTokenData(string id_token)
        {
            JwtSecurityToken token = new JwtSecurityToken(id_token);
            IdTokenData tokenData = new IdTokenData();
            tokenData.sub = token.Claims.First(claim => claim.Type == "sub" ).Value;
            tokenData.email = token.Claims.First(claim => claim.Type == "email").Value;
            return tokenData;
        }
    }
}
