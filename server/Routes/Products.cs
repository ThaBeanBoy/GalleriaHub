using Models;

using static server.Routes.APIResponse;

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
            var user = context.Items["User"] as Models.User;

            //Validating the user
            int UserID;
            if(user != null)
            {
                UserID = user.UserID;
            }
            else
            {
                UserID = -1;
            }

            // Getting filter queries
            FilterProps Filters = new(Request.Query);

            // Filter based on (public || Request User own's product), user id, min price & max price
            Console.WriteLine(UserID);
            List<Models.Product> Products = 
                DB.Products
                .Where(Product => Product.Public || (Product.UserID == UserID))
                .ToList();

            //Filter based on user id param
            if(Filters.UserID != null)
            {
                Products = Products.Where(P => P.UserID == Filters.UserID).ToList();
            }

            //Filtering based on min price
            if(Filters.MinPrice != null)
            {
                Products = Products.Where(P => Convert.ToDouble(P.Price) >= Filters.MinPrice).ToList();
            }

            //Filtering based on max price
            if(Filters.MaxPrice != null)
            {
                Products = Products.Where(P => Convert.ToDouble(P.Price) <= Filters.MaxPrice).ToList();
            }
            // Perform skip & take

            // return modified object/json

            return Products.Select(Product => Product.ResponseObj(DB));
        });

        //Getting a specific product
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
                Console.WriteLine($"Public:{Product.Public}\nLogged In: {User == null}\nOwner: {Product.UserID}\nRequester: {User.UserID}");
                if(!Product.Public && (User == null || Product.UserID != User.UserID)){
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Response.WriteAsync("Unauthorised to access product");
                }

                Response.StatusCode = StatusCodes.Status200OK;
                return Response.WriteAsJsonAsync(Product);
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

        //Delete
        group.MapDelete("/{id}", async (HttpContext context) => {
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            var User = context.Items["User"] as Models.User;

            try {
                int ProductID = int.Parse(context.GetRouteValue("id") as string ?? "error");

                Models.Product? Product = DB.Products.FirstOrDefault(P => P.ProductID == ProductID);

                // Checking if product exists
                if (Product == null) {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    await Response.WriteAsync("Couldn't find the product");
                    return;
                }

                // Checking accessibility of product and ownership
                if (User == null || Product.UserID != User.UserID) {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await Response.WriteAsync("Unauthorized to delete the product");
                    return;
                }

                // Perform the deletion
                DB.Products.Remove(Product);
                await DB.SaveChangesAsync();

                Response.StatusCode = StatusCodes.Status200OK;
            }
            catch (FormatException) {
                Response.StatusCode = StatusCodes.Status406NotAcceptable;
                await Response.WriteAsync("Product ID format is incorrect");
            }
            catch (Exception) {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                await Response.WriteAsync("Something went wrong");
            }
        });

        // (create) upload file
        group.MapPut("/{id}/asset", (HttpContext context) => {
            // todo: Check if the request user owns the product
            
            // todo: get the files from the form

            // todo: check for empty files

            /*
                todo: upload files to S3,
                todo: Add the path to the db
            */

            // todo: return the updated product
        });

        // (read) get file
        group.MapGet("/{id}/{asset}", (HttpContext context) => {
            // check if the product is private or the request user owns the product

            // return the asset
        });

        // delete files
        group.MapDelete("/{id}/{asset}", (HttpContext context) => {
            // Check if the request user owns the product

            // delegte the file
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