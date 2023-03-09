using Assignment_Wt1_Oauth.Models;

namespace Assignment_Wt1_Oauth.Contracts
{
    /// <summary>
    /// Contract for jwt-handler, referenced in dependency injection to implement dependency inversion principle.
    /// </summary>
    public interface IJwtHandler
    {
        public IdTokenData GetIdTokenData(string id_token);
    }
}
