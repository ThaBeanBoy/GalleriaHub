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
        group.MapPost("/{ListID}/{productId}", (HttpContext context) => {
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
                int ListId = int.Parse(context.GetRouteValue("ListID") as string ?? "0");

                // Find the product with the specified ID
                Models.Product? product = DB.Products.FirstOrDefault(p => p.ProductID == productId);

                if (product == null) {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    Console.WriteLine("product not found");
                    return Response.WriteAsync("Product not found");
                }

                //Check if the user already has a wishlist; create one if they don't
                Models.List? wishlist = DB.Lists.FirstOrDefault(w => w.ListID == ListId);

                if (wishlist == null) {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    Console.WriteLine("List not found");
                    return Response.WriteAsync("Could not find list");
                }

                if(wishlist.UserID != User.UserID){
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    Response.WriteAsync("Don't own the list");
                }

                // Add the product to the user's wishlist
                Models.ListItem wishlistItem = new Models.ListItem
                {
                    ProductID = product.ProductID,
                    ListID = wishlist.ListID,
                };

                DB.ListItems.Add(wishlistItem);
                DB.SaveChanges();

                Response.StatusCode = StatusCodes.Status201Created;
                return Response.WriteAsync("Product added to wishlist");
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return Response.WriteAsync("Something went wrong with the server");
            }
        });

        // Delete a product from a user's wishlist
        group.MapDelete("/{ListID}/{productId}", (HttpContext context) => {
            var (Request, Response) = (context.Request, context.Response);

            try {
                Models.User? User = context.Items["User"] as Models.User;

                if (User == null) {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Response.WriteAsync("Only logged-in users can remove products from their wishlist");
                }

                var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();

                // Parse the product ID and wishlist ID from the route
                int productId = int.Parse(context.GetRouteValue("productId") as string ?? "0");
                int ListId = int.Parse(context.GetRouteValue("ListID") as string ?? "0");

                // Find the product with the specified ID
                Models.Product? product = DB.Products.FirstOrDefault(p => p.ProductID == productId);

                if (product == null) {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    return Response.WriteAsync("Product not found");
                }

                // Find the user's wishlist based on ListID
                Models.List? wishlist = DB.Lists.FirstOrDefault(w => w.ListID == ListId);

                if (wishlist == null) {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    return Response.WriteAsync("Wishlist not found");
                }

                if (wishlist.UserID != User.UserID) {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Response.WriteAsync("You don't own the wishlist");
                }

                // Check if the product is in the user's wishlist
                Models.ListItem wishlistItem = DB.ListItems.FirstOrDefault(wi => wi.ProductID == product.ProductID && wi.ListID == ListId);

                if (wishlistItem != null) {
                    // Remove the product from the user's wishlist
                    DB.ListItems.Remove(wishlistItem);
                    DB.SaveChanges();

                    Response.StatusCode = StatusCodes.Status204NoContent;
                    return Response.WriteAsync("Product removed from wishlist");
                } else {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    return Response.WriteAsync("Product not found in the wishlist");
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return Response.WriteAsync("Something went wrong with the server");
            }
        });


        //retrieve list

        //make lists(create)

        //Delete list
        return group;
    }
}
