using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Galleria.Services;
using Models;
using server.Routes;
using static server.Routes.APIResponse;
using Utility;

namespace Routes;

public static class List
{
    public static string RouterPrefix = "/wishlists";

    public static RouteGroupBuilder ListEndpoints(this RouteGroupBuilder group)
    {
        
        // Add a product to a user's wishlist
        group.MapPost("/wishlist/add-item/{productId}", (HttpContext context) => {
            var (Request, Response) = (context.Request, context.Response);

            try {
                Models.User? User = context.Items["User"] as Models.User;

                if (User == null) {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Response.WriteAsync("Only logged-in users can add products to their wishlist");
                }

                var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();

                // Parse the product ID from the route
                int productId = int.Parse(context.GetRouteValue("productId") as string ?? "0");

                // Find the product with the specified ID
                Models.Product product = DB.Products.FirstOrDefault(p => p.ProductID == productId);

                if (product == null) {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    return Response.WriteAsync("Product not found");
                }

                // Check if the user already has a wishlist; create one if they don't
                // Models.List wishlist = DB.Wishlists.FirstOrDefault(w => w.UserId == User.UserID);

                // if (wishlist == null) {
                //     wishlist = new Models.List
                //     {
                //         UserId = User.UserID,
                //         // Set other wishlist properties
                //     };
                //     DB.Wishlists.Add(wishlist);
                // }

                // // Add the product to the user's wishlist
                // Models.WishlistItem wishlistItem = new Models.WishlistItem
                // {
                //     WishlistId = wishlist.WishlistId,
                //     ProductId = product.ProductID,
                //     Quantity = 1 // You can adjust the quantity as needed
                // };

                // DB.WishlistItems.Add(wishlistItem);
                // DB.SaveChanges();

                Response.StatusCode = StatusCodes.Status201Created;
                return Response.WriteAsync("Product added to wishlist");
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return Response.WriteAsync("Something went wrong with the server");
            }
        });

        return group;
    }
}
