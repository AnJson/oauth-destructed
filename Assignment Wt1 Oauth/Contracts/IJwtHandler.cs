using Assignment_Wt1_Oauth.Models;

namespace Assignment_Wt1_Oauth.Contracts
{
    public interface IJwtHandler
    {
        public IdTokenData GetIdTokenData(string id_token);
    }
}
