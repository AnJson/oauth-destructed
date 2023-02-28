using Assignment_Wt1_Oauth.Contracts;
using Microsoft.AspNetCore.Diagnostics;

namespace Assignment_Wt1_Oauth.Services
{
    public class ErrorService : IErrorService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ErrorService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? getErrorStatusCode()
        {
            return _httpContextAccessor.HttpContext.Response.StatusCode;
        }
    }
}
