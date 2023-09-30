using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Routes;

namespace server.Middleware;

public class UserMiddleware
{
    private readonly RequestDelegate _next;

    public UserMiddleware(RequestDelegate next)
    {
        this._next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var (Request, Response, User) = (context.Request, context.Response, context.User);
        MyAuthService MyAuthService = context.RequestServices.GetRequiredService<MyAuthService>();
        IDataProtectionProvider IDP = context.RequestServices.GetRequiredService<IDataProtectionProvider>();

        try
        {
            string? AuthCookie = Request.Headers.Cookie.FirstOrDefault(Cookie => Cookie.StartsWith("auth="));

            if(AuthCookie != null)
            {
                // Make user object null
                var Protector = IDP.CreateProtector("auth-cookie");
                string CookiePayload = Protector.Unprotect(AuthCookie.Split("=").Last());
                int UserID = int.Parse(CookiePayload.Split(":").Last());

                Console.WriteLine($"User ID: {UserID}");
            
                List<Claim> Claims = new(){
                    new Claim(ClaimTypes.NameIdentifier, UserID.ToString())
                };
                var Identity = new ClaimsIdentity(Claims);
                context.User = new ClaimsPrincipal(Identity);
            }
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            await _next(context);
        }

    }
}

public static class UserMiddlewareExtension {
    public static IApplicationBuilder UseUserMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UserMiddleware>();
    }
}