using Azure;
using Models;

namespace Routes;

public static class Product{

    public static string RouterPrefix = "/products";

    public static RouteGroupBuilder ProductEndpoints(this RouteGroupBuilder group){
        // Create
        group.MapPost("/new-product", (HttpContext context) => {
            var (Request, Response) = (context.Request, context.Response);
            try{
                var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();

                string? ProductName = Convert.ToString(Request.Form["name"]).Trim();

                if(string.IsNullOrEmpty(ProductName)){
                    Response.StatusCode = StatusCodes.Status406NotAcceptable;
                    return Response.WriteAsync("You need to provide a name");
                }

                Models.Product NewProduct = new Models.Product(){
                    ArtistID = 1,
                    ProductName = Request.Form["name"]
                };

                Response.StatusCode = StatusCodes.Status501NotImplemented;
                return Response.WriteAsync("Supposed to handle new products");
            }catch(Exception e){
                Console.WriteLine(e.Message);
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return Response.WriteAsync("Something went wrong with the server");
            }
        });
        
        // Retrieve
        group.MapGet("/", (HttpContext context)=>{
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();

            Response.StatusCode = StatusCodes.Status501NotImplemented;
            Response.WriteAsync("Supposed to handle getting multiple products");

        });

        group.MapGet("/{id}", (HttpContext context) => {
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            
            Response.StatusCode = StatusCodes.Status501NotImplemented;
            Response.WriteAsync("Not implmeneted yet");
        });

        // Update
        group.MapPut("/{id}", (HttpContext context) => {
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            
            Response.StatusCode = StatusCodes.Status501NotImplemented;
            Response.WriteAsync("Not implmeneted yet");
        });

        // Delete
        group.MapDelete("/{id}", (HttpContext context) => {
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            
            Response.StatusCode = StatusCodes.Status501NotImplemented;
            Response.WriteAsync("Not implmeneted yet");
        });

        return group;
    }

}