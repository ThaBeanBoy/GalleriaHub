
using Models;

// tutorial: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-7.0
namespace GalleriaMiddleware;

public class DatabaseConnectionTestMiddleware
{
    private readonly RequestDelegate _next; 

    public DatabaseConnectionTestMiddleware(RequestDelegate next)
    {
        this._next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            if(DB == null || !DB.Database.CanConnect())
            throw new Exception();

            await _next(context);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);

            HttpResponse Res = context.Response; 
            Res.StatusCode = StatusCodes.Status500InternalServerError;
            await Res.WriteAsync("Couldn't establish connection to the database");
        }

    }
}

public static class DatabaseConnectionMiddlewareExtenstion
{
    public static IApplicationBuilder UseDatabaseConnectionTest(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<DatabaseConnectionTestMiddleware>();
    }
}