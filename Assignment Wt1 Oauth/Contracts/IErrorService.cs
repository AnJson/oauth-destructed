namespace Assignment_Wt1_Oauth.Contracts
{
    /// <summary>
    /// Contract for error-service, referenced in dependency injection to implement dependency inversion principle.
    /// </summary>
    public interface IErrorService
    {
        public int? getErrorStatusCode();
    }
}
