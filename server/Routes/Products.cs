namespace Routes;

public static class Product{

    public static string RouterPrefix = "/products";

    public static RouteGroupBuilder ProductEdpoints(this RouteGroupBuilder group){
        group.MapPost("/nre-product", (HttpContext context) =>{
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();

            // Response.StatusCode = StatusCodes.
        })
        
        return group;
    }

}