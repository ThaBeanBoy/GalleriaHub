using Models;

namespace Routes;

public static class Product{

    public static string RouterPrefix = "/products";

    public static RouteGroupBuilder ProductEndpoints(this RouteGroupBuilder group){
        // Create
        group.MapPost("/new-product", (HttpContext context) => {
            var (Request, Response) = (context.Request, context.Response);
            
            try{
                Models.User? User = context.Items["User"] as Models.User;

                if(User == null){
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Response.WriteAsync("Only logged in users can make products");
                }

                var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();

                string? ProductName = Convert.ToString(Request.Form["name"]).Trim();

                if(string.IsNullOrEmpty(ProductName)){
                    Response.StatusCode = StatusCodes.Status406NotAcceptable;
                    return Response.WriteAsync("You need to provide a name");
                }

                Models.Product NewProduct = new Models.Product(){
                    UserID = User.UserID, 
                    ProductName = ProductName,
                    CreatedOn = DateTime.Now,
                    LastUpdate = DateTime.Now,
                    Public = false
                };

                DB.Products.Add(NewProduct);
                DB.SaveChanges();

                Response.StatusCode = StatusCodes.Status201Created;
                return Response.WriteAsJsonAsync(NewProduct);
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

            // Getting filter queries
            FilterProps Filters = new(Request.Query);
            Response.StatusCode = StatusCodes.Status200OK;

            return DB.Products/* .Where(Product => Product.Public) */;
        });

        group.MapGet("/{id}", (HttpContext context) => {
            var (Request, Response) = (context.Request, context.Response);
            
            try{
                var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
                
                int ProductID = int.Parse(context.GetRouteValue("id") as string ?? "error");

                Models.Product? Product = DB.Products.FirstOrDefault(P => P.ProductID == ProductID);

                // Checking if product exists
                if(Product == null){
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    return Response.WriteAsync("Coudln't find the product");
                }

                // Checking accessibility of product || If user requesting, then allow access to product
                if(!Product.Public /* || Product.ArtistID == User Artist ID */){
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Response.WriteAsync("Unauthorised to access product");
                }

                Response.StatusCode = StatusCodes.Status200OK;
                return Response.WriteAsync($"Supposed to get {context.GetRouteValue("id")}");
            }
            catch(FormatException){
                Response.StatusCode = StatusCodes.Status406NotAcceptable;
                return Response.WriteAsync("Product ID format is incorrect");    
            }
            catch(Exception){
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return Response.WriteAsync("Something went wrong");
            }
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

    private class FilterProps {
        public bool Verified { get; }
        public int? ArtistID { get; }
        public double? MinPrice { get; }
        public double? MaxPrice { get; }

        public FilterProps(IQueryCollection Queries){
            // Checking out verified query
            try{
                Verified = Convert.ToBoolean(Queries["verified"]);
            }
            catch(Exception) {
                Verified = false;
            }

            // Checking ArtistID
            try{
                ArtistID = int.Parse(Queries["artistid"]);
            }
            catch(Exception){
                ArtistID = null;
            }

            // Checking the min price
            try{
                MinPrice = double.Parse(Queries["min_price"]);
            }
            catch(Exception){
                MinPrice = null;
            }

            // Checking the max price
            try{
                MaxPrice = double.Parse(Queries["max_price"]);
            }
            catch(Exception){
                MaxPrice = null;
            }

        }
    
        public static bool Filter(Object? FilterProp, Object FilterValue){
            if(FilterProp != null){
                return FilterProp == FilterValue;
            }

            return true;
        }
    }
}