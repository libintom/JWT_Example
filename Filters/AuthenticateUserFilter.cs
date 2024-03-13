using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Security.Authentication;
using System.Text;

namespace JWT_Example.Filters
{
    public class AuthenticateUserAttribute : Attribute, IAuthorizationFilter
    {
        public AuthenticateUserAttribute()
        {
            
        }
        public virtual void OnAuthorization(AuthorizationFilterContext authContext)
        {
            try
            {
                if (authContext.HttpContext.Request.Headers.Authorization[0] != null)
                {
                    // Gets header parameters  
                    string authCredentialsEncoded = authContext.HttpContext.Request.Headers.Authorization[0].Replace("Basic ","");
                    string authCredentialsDecoded = Encoding.UTF8.GetString(Convert.FromBase64String(authCredentialsEncoded));

                    // Gets username and password  
                    string username = authCredentialsDecoded.Split(':')[0];
                    string password = authCredentialsDecoded.Split(':')[1];

                    if (username != "admin" || password != "admin123")
                    {
                        //returns unauthorized error
                        authContext.Result = new UnauthorizedObjectResult(string.Empty);
                        return;
                    }
                }                
            }
            catch
            {
                authContext.Result = new UnauthorizedObjectResult(string.Empty);
                return;
            }

        }

    }
}
