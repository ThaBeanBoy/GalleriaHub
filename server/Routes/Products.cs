<<<<<<< HEAD
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Http.Extensions;
using Models;
using Microsoft.AspNetCore.Authentication;
using Cart;
using Galleria.Middleware;
=======
using Galleria.Services;
using Models;
using server.Routes;
using static server.Routes.APIResponse;
>>>>>>> lists-ui

namespace Routes;

public static class Product
{

    public static string RouterPrefix = "/products";

    public static RouteGroupBuilder ProductEndpoints(this RouteGroupBuilder group)
    {
        // Create
        group.MapPost("/new-product", (HttpContext context) =>
        {
            var (Request, Response) = (context.Request, context.Response);

            Console.WriteLine("Making new product");

            try
            {
                Models.User? User = context.Items["User"] as Models.User;

                if (User == null)
                {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Response.WriteAsync("Only logged in users can make products");
                }

                var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();

                string? ProductName = Convert.ToString(Request.Form["name"]).Trim();

                if (string.IsNullOrEmpty(ProductName))
                {
                    Response.StatusCode = StatusCodes.Status406NotAcceptable;
                    return Response.WriteAsync("You need to provide a name");
                }

                // Checking the product price
                double ProductPrice = 0;
                try
                {
                    ProductPrice = Convert.ToDouble(Convert.ToString(Request.Form["price"]).Trim() ?? "0");
                }
                catch (FormatException ex)
                {
                    Response.StatusCode = StatusCodes.Status406NotAcceptable;
                    return Response.WriteAsync(ex.Message);
                }
                catch (Exception) {/* Do nothing */}

                // Checking for stock input
                int ProductStock = 0;
                try
                {
                    ProductStock = Convert.ToInt32(Convert.ToString(Request.Form["stock"]).Trim() ?? "0");
                }
                catch (FormatException ex)
                {
                    Response.StatusCode = StatusCodes.Status406NotAcceptable;
                    return Response.WriteAsync(ex.Message);
                }
                catch (Exception) {/* Do nothing */}

                Models.Product NewProduct = new Models.Product
                {
                    UserID = User.UserID,
                    ProductName = ProductName,
                    Price = new decimal(ProductPrice),
                    StockQuantity = ProductStock,
                    Description = $"<p><strong>{ProductName}</strong> is a product that is ...</p>",
                    CreatedOn = DateTime.Now,
                    LastUpdate = DateTime.Now,
                    Public = false
                };

                DB.Products.Add(NewProduct);
                DB.SaveChanges();

                Response.StatusCode = StatusCodes.Status201Created;
                return Response.WriteAsJsonAsync(NewProduct.ResponseObj(context));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return Response.WriteAsync("Something went wrong with the server");
            }
        });

        // Retrieve
        group.MapGet("/", (HttpContext context) =>
        {
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            var user = context.Items["User"] as Models.User;

            //Validating the user
            int UserID;
            if (user != null)
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
            if (Filters.UserID != null)
            {
                Products = Products.Where(P => P.UserID == Filters.UserID).ToList();
            }

            //Filtering based on min price
            if (Filters.MinPrice != null)
            {
                Products = Products.Where(P => Convert.ToDouble(P.Price) >= Filters.MinPrice).ToList();
            }

            //Filtering based on max price
            if (Filters.MaxPrice != null)
            {
                Products = Products.Where(P => Convert.ToDouble(P.Price) <= Filters.MaxPrice).ToList();
            }
            // Perform skip & take

            // return modified object/json

            return Products.Select(Product => Product.ResponseObj(context));
        });

        //Getting a specific product
        group.MapGet("/{id}", (HttpContext context) =>
        {
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            var User = context.Items["User"] as Models.User;

            try
            {

                int ProductID = int.Parse(context.GetRouteValue("id") as string ?? "error");

                Models.Product? Product = DB.Products.FirstOrDefault(P => P.ProductID == ProductID);

                // Checking if product exists
                if (Product == null)
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    return Response.WriteAsync("Coudln't find the product");
                }

                if (!Product.Public && (User == null || Product.UserID != User.UserID))
                {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Response.WriteAsync("Unauthorised to access product");
                }

                Response.StatusCode = StatusCodes.Status200OK;
                return Response.WriteAsJsonAsync(Product.ResponseObj(context));
            }
            catch (FormatException)
            {
                Response.StatusCode = StatusCodes.Status406NotAcceptable;
                return Response.WriteAsync("Product ID format is incorrect");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return Response.WriteAsync("Something went wrong");
            }
        });

        // Update
<<<<<<< HEAD
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
=======
        group.MapPut("/{id}", async (HttpContext context) =>
        {
            Console.WriteLine("Updating");
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            Models.User? User = context.Items["User"] as Models.User;

            try
            {
                if (User == null)
                {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await Response.WriteAsync("Not logged in");
                    return;
                }

                int ProductID = int.Parse(context.GetRouteValue("id") as string ?? "error");

                Models.Product? Product = DB.Products.FirstOrDefault(Product => Product.ProductID == ProductID);

                if (Product == null)
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    await Response.WriteAsync("Product not found");
                    return;
                }

                UpdateProductModel? Update = await Request.ReadFromJsonAsync<UpdateProductModel>();

                if (Update == null)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    await Response.WriteAsync("Couldn't recognise body");
                    return;
                }

                // Updating product name
                if (Update.productName != null)
                {
                    // Validatign the new product name
                    if (Update.productName.Trim() == "")
                    {
                        Response.StatusCode = StatusCodes.Status406NotAcceptable;
                        await Response.WriteAsync("Name of product cannot be empty");
                        return;
                    }

                    Product.ProductName = Update.productName;

                    Product.LastUpdate = DateTime.Now;
                }

                // updating price
                if (Update.price != null)
                {
                    try
                    {
                        double NewPrice = double.Parse(Update.price);

                        Product.Price = new decimal(NewPrice);

                        Product.LastUpdate = DateTime.Now;
                    }
                    catch (FormatException ex)
                    {
                        Response.StatusCode = StatusCodes.Status406NotAcceptable;
                        await Response.WriteAsync(ex.Message);
                        return;
                    }
                }

                // Update stock
                if (Update.stock != null)
                {
                    try
                    {
                        int NewStock = int.Parse(Update.stock);

                        Product.StockQuantity = NewStock;

                        Product.LastUpdate = DateTime.Now;
                    }
                    catch (FormatException ex)
                    {
                        Response.StatusCode = StatusCodes.Status406NotAcceptable;
                        await Response.WriteAsync(ex.Message);
                    }
                }

                // udating product description
                if (Update.description != null)
                {
                    Product.Description = Update.description;
                    Product.LastUpdate = DateTime.Now;
                }

                if (Update.Public != null)
                {
                    Product.Public = Update.Public;
                }

                // Updating the DB
                DB.Products.Update(Product);
                DB.SaveChanges();

                await Response.WriteAsJsonAsync(Product.ResponseObj(context));
            }
            catch (FormatException)
            {
                Response.StatusCode = StatusCodes.Status406NotAcceptable;
                await Response.WriteAsync("Incorrect format for product id");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                await Response.WriteAsync("Something went wrong");
            }
>>>>>>> lists-ui
        });

        //Delete
        group.MapDelete("/{id}", async (HttpContext context, IWebHostEnvironment env) =>
        {
            Console.WriteLine("running delete");
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            var S3 = context.RequestServices.GetRequiredService<S3BucketService>();
            var User = context.Items["User"] as Models.User;

            try
            {
                int ProductID = int.Parse(context.GetRouteValue("id") as string ?? "error");

                Models.Product? Product = DB.Products.FirstOrDefault(P => P.ProductID == ProductID);

                // Checking if product exists
                if (Product == null)
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    await Response.WriteAsync("Couldn't find the product");
                    return;
                }

                // Checking accessibility of product and ownership
                if (User == null || Product.UserID != User.UserID)
                {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await Response.WriteAsync("Unauthorized to delete the product");
                    return;
                }

                // Deleting product files
                DB.ProductFiles
                    .Where(PF => PF.ProductID == Product.ProductID)
                    .ToList()
                    .ForEach(PF =>
                    {
                        // Deleting from the DB
                        DB.ProductFiles.Remove(PF);

                        // Delete from S3 & static folder
                        S3.Delete(env, PF.FileKey);
                    });

                // Perform the deletion
                DB.Products.Remove(Product);

                // Saving DB Changes
                await DB.SaveChangesAsync();

                Response.StatusCode = StatusCodes.Status200OK;
            }
            catch (FormatException)
            {
                Response.StatusCode = StatusCodes.Status406NotAcceptable;
                await Response.WriteAsync("Product ID format is incorrect");
            }
            catch (Exception)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                await Response.WriteAsync("Something went wrong");
            }
        });

        group.MapPost("/{ProductID}/cart", (HttpContext context) => {
            var (request, response) = (context.Request, context.Response);
            var db = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();

            
            dynamic id = request.ReadFromJsonAsync<OrderClass>();
            var verified = id.Result.Users;

            if (verified != null) {
                try
                {
                    var orders = new Order
                    {
                        OrderDate = id.Result.OrderDate,
                        Customer = id.Result.Users,
                        Discount = id.Result.Discounts,
                        OrderID = id.Result.OrderID
                    };
                    var item = new OrderItem
                    {
                        Product = id.Result.Products,
                        Order = orders,
                        Quantity = id.Result.Quantity,
                        Price = id.Result.Price
                    };
                    
                    db.Orders.Add(orders);
                    db.OrderItems.Add(item);
                    db.SaveChanges();
                    response.StatusCode = StatusCodes.Status200OK;
                    response.WriteAsJsonAsync(orders);
                }
                catch (Exception)
                {
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.WriteAsync("No order found");
                }
            }
        });

        group.MapDelete("/{ProductID}/cart", (HttpContext http) =>
        {
            var (request, response) = (http.Request, http.Response);
            var db = http.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            var id = request.ReadFromJsonAsync<OrderClass>();
            var verified = id.Result.Users;

            if (verified != null)
            {
                var items = (from o in db.Orders
                             where o.OrderID.Equals(id.Result.OrderID)
                             select o).FirstOrDefault();
                try
                {
                    if (items != null) {
                        var orders = new Order
                        {
                            OrderDate = id.Result.OrderDate,
                            Customer = id.Result.Users,
                            Discount = id.Result.Discounts,
                            OrderID = id.Result.OrderID
                        };
                        var item = new OrderItem
                        {
                            Product = id.Result.Products,
                            Order = orders,
                            Quantity = id.Result.Quantity,
                            Price = id.Result.Price
                        };
                        db.Orders.Update(orders);
                        db.OrderItems.Update(item);
                        db.SaveChanges();
                    }
                    response.StatusCode = StatusCodes.Status200OK;
                    response.WriteAsJsonAsync(id);
                }
                catch (Exception)
                {
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.WriteAsync("Could not delete products");
                }
                

            }

        });

        return group;
    }

    private class UpdateProductModel
    {
        public string? productName { get; set; }
        public string? price { get; set; }
        public string? stock { get; set; }
        public string? description { get; set; }
        public bool Public { get; set; }
    }

    private class FilterProps
    {
        public bool Verified { get; }

        public int? Skip { get; }
        public int? Take { get; }
        public int? UserID { get; }
        public double? MinPrice { get; }
        public double? MaxPrice { get; }

        public FilterProps(IQueryCollection Queries)
        {
            // Checking out verified query
            try
            {
                Verified = Convert.ToBoolean(Queries["verified"]);
            }
            catch (Exception)
            {
                Verified = false;
            }

            //Skip
            try
            {
                Skip = Convert.ToInt32(Queries["skip"]);
            }
            catch (Exception)
            {
                Skip = null;
            }

            //Take
            try
            {
                Take = Convert.ToInt32(Queries["take"]);
            }
            catch (Exception)
            {
                Take = null;
            }

            // Checking ArtistID
            try
            {
                UserID = int.Parse(Queries["userid"]);
            }
            catch (Exception)
            {
                UserID = null;
            }

            // Checking the min price
            try
            {
                MinPrice = double.Parse(Queries["min_price"]);
            }
            catch (Exception)
            {
                MinPrice = null;
            }

            // Checking the max price
            try
            {
                MaxPrice = double.Parse(Queries["max_price"]);
            }
            catch (Exception)
            {
                MaxPrice = null;
            }

        }

        public static bool Filter(Object? FilterProp, Object FilterValue)
        {
            if (FilterProp != null)
            {
                return FilterProp == FilterValue;
            }

            return true;
        }
    }
}