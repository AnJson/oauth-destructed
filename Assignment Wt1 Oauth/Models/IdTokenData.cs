namespace Assignment_Wt1_Oauth.Models
{
    /// <summary>
    /// Model to wrap needed data from id_token.
    /// </summary>
    public class IdTokenData
    {
        public string? sub { get; set; }
        public string? email { get; set; }
    }
}
