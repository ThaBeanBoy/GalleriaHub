using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Models;

namespace server.Middleware;

public class UserMiddleware
{
    private readonly RequestDelegate _next; 
    
    public UserMiddleware(RequestDelegate next){
        this._next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var (Request, Response) = (context.Request, context.Response);
        var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();

        //Getting the UserID
        Claim? UserIDClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        try
        {
            if(UserIDClaim != null)
            {
                User? User = DB.Users.FirstOrDefault(U => U.UserID == int.Parse(UserIDClaim.Value));
                if(User != null)
                {
                    context.Items["User"] = User;
                }
            }
        }
        catch(Exception)
        {
            /* Do nothing */
        }
        finally
        {
            await _next(context);
        }
    }
}

public static class UserMiddlewareExtension
{
    public static IApplicationBuilder UseUserMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UserMiddleware>();
    }
}