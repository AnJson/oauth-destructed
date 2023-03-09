using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Assignment_Wt1_Oauth.Utils
{
    /// <summary>
    /// Handles jwt-tokens.
    /// </summary>
    public class JwtHandler : IJwtHandler
    {
        /// <summary>
        /// Decodes jwt-token.
        /// </summary>
        /// <param name="id_token">Token to decode.</param>
        /// <returns>Wrapper model for the decoded data.</returns>
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
