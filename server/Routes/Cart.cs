using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using static server.Routes.APIResponse;

namespace Routes;

public static class Cart
{
    public static string RouterPrefix = "/cart";

    public static RouteGroupBuilder CartEndpoints(this RouteGroupBuilder group)
    {

        // READ
        group.MapGet("/", async (HttpContext context) =>
        {
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            var User = context.Items["User"] as Models.User;

            try
            {
                if (User == null)
                {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await Response.WriteAsync("Need to be logged in");
                    return;
                }

                List<UserCartItem> Cart = DB.UserCartItems.Where(CartItem => CartItem.UserID == User.UserID).ToList();

                await Response.WriteAsJsonAsync(Cart.ResponseObj(context));
            }
            catch (Exception)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                await Response.WriteAsync("Something went wrong");
            }
        });

        // UPDATE
        // Add to cart
        group.MapPut("/add", async (HttpContext context) =>
        {
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            var User = context.Items["User"] as Models.User;

            try
            {
                if (User == null)
                {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await Response.WriteAsync("Need to be logged in");
                    return;
                }

                CartQueryParams Params = new(Request.Query);

                if (Params.ProductID == null)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    await Response.WriteAsync("Need to provide a product ID");
                    return;
                }

                Models.Product? Product = DB.Products.FirstOrDefault(Product => Product.ProductID == Params.ProductID);

                if (Product == null)
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    await Response.WriteAsync("Product not found");
                    return;
                }

                if (Product.StockQuantity < 1)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    await Response.WriteAsync("Can't add a product with a quantity less than 1");
                    return;
                }

                // check if product alread in cart
                var Cart = DB.UserCartItems.Where(UserCartItems => UserCartItems.UserID == User.UserID).ToList();

                bool AlreadyInCart = DB.UserCartItems.FirstOrDefault(UserCartItem =>
                    UserCartItem.UserID == User.UserID &&
                    UserCartItem.ProductID == Product.ProductID) != null;

                if (AlreadyInCart)
                {
                    await Response.WriteAsJsonAsync(Cart.ResponseObj(context));
                    return;
                }

                // Adding product in cart
                DB.UserCartItems.Add(new UserCartItem
                {
                    UserID = User.UserID,
                    ProductID = Product.ProductID,
                    Quantity = 1 // default quantity is 1
                });

                DB.SaveChanges();

                Cart = DB.UserCartItems.Where(UserCartItems => UserCartItems.UserID == User.UserID).ToList();

                Response.StatusCode = StatusCodes.Status200OK;
                await Response.WriteAsJsonAsync(Cart.ResponseObj(context));
            }
            catch (Exception)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                await Response.WriteAsync("Something went wrong");
            }
        });

        // update quantity
        group.MapPut("/update", async (HttpContext context) =>
        {
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            var User = context.Items["User"] as Models.User;

            try
            {
                // Check if user is logged in
                if (User == null)
                {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await Response.WriteAsync("Need to be logged in");
                    return;
                }

                CartQueryParams Params = new(Request.Query);

                Models.Product? Product = DB.Products.FirstOrDefault(Product => Product.ProductID == Params.ProductID);

                // Check for product
                if (Product == null)
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    await Response.WriteAsync("Product not found");
                    return;
                }

                // Checking the quantity
                if (Params.Quantity == null)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    await Response.WriteAsync("Need an update quantity");
                    return;
                }

                // Checking the product's stock
                if (Params.Quantity > Product.StockQuantity)
                {
                    Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
                    await Response.WriteAsync("Quantity can't exceed the stock");
                    return;
                }

                // Checking the quantity value
                if (Params.Quantity < 1)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    await Response.WriteAsync("Can't have quantity less than 1");
                    return;
                }

                // inserting product into cart if not in cart
                var ProductNotInCart = DB.UserCartItems.FirstOrDefault(CartItem => CartItem.UserID == User.UserID && CartItem.ProductID == Product.ProductID) == null;
                if (ProductNotInCart)
                {
                    DB.UserCartItems.Add(new UserCartItem
                    {
                        UserID = User.UserID,
                        ProductID = Product.ProductID,
                        Quantity = 1 // default quantity is 1
                    });

                    DB.SaveChanges();
                }

                // Updating
                UserCartItem? CartItem = DB.UserCartItems.FirstOrDefault(CartItem =>
                    CartItem.UserID == User.UserID && CartItem.ProductID == Product.ProductID);

                // Updating
                CartItem.Quantity = Params.Quantity ?? 1;

                DB.UserCartItems.Update(CartItem);

                DB.SaveChanges();

                // Returning the cart
                var Cart = DB.UserCartItems.Where(UserCartItems => UserCartItems.UserID == User.UserID).ToList();
                await Response.WriteAsJsonAsync(Cart.ResponseObj(context));
            }
            catch (Exception)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                await Response.WriteAsync("Something went wrong");
            }
        });

        // remove from cart
        group.MapDelete("/delete", async (HttpContext context) =>
        {
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            var User = context.Items["User"] as Models.User;

            try
            {
                // Check if user is logged in
                if (User == null)
                {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await Response.WriteAsync("Need to be logged in");
                    return;
                }

                CartQueryParams Params = new(Request.Query);

                Models.Product? Product = DB.Products.FirstOrDefault(Product => Product.ProductID == Params.ProductID);

                // Check for product
                if (Product == null)
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    await Response.WriteAsync("Product not found");
                    return;
                }

                UserCartItem? CartItem = DB.UserCartItems.FirstOrDefault(CartItem => CartItem.UserID == User.UserID && CartItem.ProductID == Product.ProductID);

                var Cart = DB.UserCartItems.Where(UserCartItems => UserCartItems.UserID == User.UserID).ToList();

                // Check if the product is in cart
                if (CartItem == null)
                {
                    await Response.WriteAsJsonAsync(Cart.ResponseObj(context));
                    return;
                }

                DB.UserCartItems.Remove(CartItem);
                DB.SaveChanges();

                Cart = DB.UserCartItems.Where(UserCartItems => UserCartItems.UserID == User.UserID).ToList();

                await Response.WriteAsJsonAsync(Cart.ResponseObj(context));
            }
            catch (Exception)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                await Response.WriteAsync("Something went wrong");
            }
        });

        return group;
    }

    private class CartQueryParams
    {
        public int? ProductID { get; }
        public int? Quantity { get; }

        public CartQueryParams(IQueryCollection Queries)
        {
            // Getting the product id
            try
            {
                ProductID = Convert.ToInt32(Queries["productID"]);
            }
            catch (Exception)
            {
                ProductID = null;
            }

            // Getting the Quantity value
            try
            {
                Quantity = Convert.ToInt32(Queries["quantity"]);
            }
            catch (Exception)
            {
                Quantity = null;
            }
        }
    }
}