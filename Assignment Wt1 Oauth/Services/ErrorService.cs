using Assignment_Wt1_Oauth.Contracts;

namespace Assignment_Wt1_Oauth.Services
{
    /// <summary>
    /// Service that handles errrs.
    /// </summary>
    public class ErrorService : IErrorService
    {
        /// <summary>
        /// Injected service.
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Constructor recieving injected services.
        /// </summary>
        /// <param name="httpContextAccessor">Service to be able to access the response in the httpcontext.</param>
        public ErrorService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get status-code of the response.
        /// </summary>
        /// <returns>Status code.</returns>
        public int? getErrorStatusCode()
        {
            return _httpContextAccessor.HttpContext.Response.StatusCode;
        }
    }
}
