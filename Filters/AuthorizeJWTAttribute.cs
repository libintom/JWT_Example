using JWT_Example.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JWT_Example.Filters
{
    public class AuthorizeJWTAttribute : TypeFilterAttribute
    {
        public AuthorizeJWTAttribute() : base(typeof(AuthorizeJWTFilter))
        {
        }
    }

    public class AuthorizeJWTFilter : IAuthorizationFilter
    {
        private IJWTService _jwtService;
        public AuthorizeJWTFilter(IJWTService jwtService)
        {
            _jwtService = jwtService;
        }
        public virtual void OnAuthorization(AuthorizationFilterContext authContext)
        {
            try
            {
                if (authContext.HttpContext.Request.Headers.Authorization[0] == null)
                {
                    authContext.Result = new UnauthorizedObjectResult(string.Empty);
                    return;
                }
                else
                {
                    // Get JWT Token  
                    string jwtTokenBearer = authContext.HttpContext.Request.Headers.Authorization[0];
                    string jwtToken = jwtTokenBearer.Replace("Bearer ", "");

                    var validationResult = _jwtService.ValidateJwtToken(jwtToken, "JWTAudience"); // Validate JWT Token

                    if (!validationResult.Result.IsValid)
                    {
                        // returns unauthorized error  
                        authContext.Result = new UnauthorizedObjectResult(string.Empty);
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                authContext.Result = new UnauthorizedObjectResult(string.Empty);
                return;
            }
        }
    }
}
