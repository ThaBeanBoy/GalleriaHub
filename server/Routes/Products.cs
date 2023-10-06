using System.Reflection.Metadata;
using Microsoft.AspNetCore.Http.Extensions;
using Models;

namespace Routes;

public static class Product{

    public static string RouterPrefix = "/products";

    public static RouteGroupBuilder ProductEndpoints(this RouteGroupBuilder group){
        // Create
        group.MapPost("/new-product", (HttpContext context) => {
            var (Request, Response) = (context.Request, context.Response);
            
            Console.WriteLine("Making new product");
            
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

                // Checking the product price
                double ProductPrice = 0;
                try
                {
                    ProductPrice = Convert.ToDouble(Convert.ToString(Request.Form["price"]).Trim() ?? "0");
                } 
                catch(FormatException ex)
                {
                    Response.StatusCode = StatusCodes.Status406NotAcceptable;
                    return Response.WriteAsync(ex.Message);
                }
                catch(Exception){/* Do nothing */}

                // Checking for stock input
                int ProductStock = 0;
                try{
                    ProductStock = Convert.ToInt32(Convert.ToString(Request.Form["stock"]).Trim() ?? "0");
                }
                catch(FormatException ex)
                {
                    Response.StatusCode = StatusCodes.Status406NotAcceptable;
                    return Response.WriteAsync(ex.Message);
                }
                catch(Exception) {/* Do nothing */}

                Models.Product NewProduct = new Models.Product{
                    UserID = User.UserID, 
                    ProductName = ProductName,
                    Price = new decimal(ProductPrice),
                    StockQuantity = ProductStock,
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
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            var User = context.Items["User"] as Models.User;

            try{
                
                int ProductID = int.Parse(context.GetRouteValue("id") as string ?? "error");

                Models.Product? Product = DB.Products.FirstOrDefault(P => P.ProductID == ProductID);

                // Checking if product exists
                if(Product == null){
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    return Response.WriteAsync("Coudln't find the product");
                }

                // Checking accessibility of product || If user owns product, then allow access to product
                if(!Product.Public && User != null && Product.UserID != User.UserID){
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
        group.MapPut("/{id}", (HttpContext context) =>
        {
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            var bodyStream = new StreamReader(Request.Body);
            try {
                int? id = Convert.ToInt32(Request.RouteValues["id"].ToString());
                
                var product = (from p in DB.Products
                        where id == p.ProductID
                        select p).FirstOrDefault();

                if (product != null)
                {
                    dynamic keys = bodyStream.ReadToEndAsync();
                    dynamic items = keys.split();
                    bool hasChanged = ((items["Name"].ToString() != product.ProductName) || 
                    (Convert.ToDecimal(items["Price"].ToString()) != product.Price) || 
                    (Convert.ToInt32(items["Quantity"].ToString()) != product.StockQuantity) || 
                    (Convert.ToBoolean(items["AccountStatus"].ToString()) != product.Public));
                    if (hasChanged) {
                        var prod = new Models.Product
                        {
                            ProductName = items["Name"].ToString(),
                            Price = Convert.ToDecimal(items["Price"].ToString()),
                            StockQuantity = Convert.ToInt32(items["Quantity"].ToString()),
                            Public = Convert.ToBoolean(items["AccountStatus"].ToString()),
                            LastUpdate = DateTime.Now
                        };
                        
                        DB.SaveChanges();
                        Response.StatusCode = StatusCodes.Status200OK;
                        Response.WriteAsJsonAsync(prod);
                    }
                }
                else {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    Response.WriteAsync("Invalid ID");
                }
            }
            catch (Exception) {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync("Something Happened");
            }

            Response.StatusCode = StatusCodes.Status501NotImplemented;
            Response.WriteAsync("Not implmeneted yet");
        });

        // Delete | Archive
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

        public int? Skip { get; }
        public int? Take { get; }
        public int? UserID { get; }
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

            //Skip
            try{
                Skip = Convert.ToInt32(Queries["skip"]);
            }
            catch(Exception) {
                Skip = null;
            }

            //Take
            try{
                Take = Convert.ToInt32(Queries["take"]);
            }
            catch(Exception) {
                Take = null;
            }

            // Checking ArtistID
            try{
                UserID = int.Parse(Queries["userid"]);
            }
            catch(Exception){
                UserID = null;
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