using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace Routes;

public static class Sales
{
    public static string RouterPrefix = "/sales";

    public static RouteGroupBuilder SalesEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", (HttpContext context) =>
        {
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            var User = context.Items["User"] as Models.User;

            try
            {
                if (User == null)
                {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Response.WriteAsync("Need to log in");
                }

                Response.StatusCode = StatusCodes.Status501NotImplemented;
                return Response.WriteAsync("Not implemented");
            }
            catch (Exception)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return Response.WriteAsync("Something went wrong");
            }
        });

        return group;
    }
}