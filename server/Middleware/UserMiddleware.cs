using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Galleria.Services;
using Models;

namespace Galleria.Middleware;

public class UserMiddleware
{
    private readonly RequestDelegate _next; 
    
    public UserMiddleware(RequestDelegate next){
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var (Request, Response) = (context.Request, context.Response);
        var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
        var JWT = context.RequestServices.GetRequiredService<JWTService>();

        try{
            string? AuthHeader = Request.Headers["Authorization"].ToString();
            
            if(string.IsNullOrEmpty(AuthHeader) && !AuthHeader.StartsWith("Bearer ")) throw new Exception();

            string JwtTokenString = AuthHeader.Substring("Bearer ".Length);
            if(string.IsNullOrEmpty(JwtTokenString)) throw new Exception();

            ClaimsPrincipal? UserClaimsPrinciple = JWT.GetClaims(JwtTokenString);
            if(UserClaimsPrinciple == null) throw new Exception();

            Claim? UserIDClaim = UserClaimsPrinciple.FindFirst(ClaimTypes.NameIdentifier);
            if(UserIDClaim == null) throw new Exception();

            User? User = DB.Users.FirstOrDefault(U => U.UserID == int.Parse(UserIDClaim.Value));
            if(User != null)
            {
                context.Items["User"] = User;
            }
        }catch(Exception){
            // Do nothing
        }finally{
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