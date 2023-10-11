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
                return Response.WriteAsJsonAsync(wishlist.ResponseObj(context));
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
                    return Response.WriteAsJsonAsync(wishlist.ResponseObj(context));
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

        // Retrieve a user's wishlist
        group.MapGet("/{ListID}", (HttpContext context) => {
            var (Request, Response) = (context.Request, context.Response);

            try {
                Models.User? User = context.Items["User"] as Models.User;

                if (User == null) {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Response.WriteAsync("Only logged-in users can access wishlists");
                }

                var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();

                // Parse the `ListID` from the route
                int listId = int.Parse(context.GetRouteValue("ListID") as string ?? "0");

                // Find the user's wishlist based on `ListID`
                Models.List? wishlist = DB.Lists.FirstOrDefault(w => w.ListID == listId);

                if (wishlist == null) {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    return Response.WriteAsync("Wishlist not found");
                }

                if (wishlist.UserID != User.UserID) {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Response.WriteAsync("You don't own the wishlist");
                }

                // Get the list items based on the `ListID`
                List<Models.ListItem> listItems = DB.ListItems.Where(li => li.ListID == listId).ToList();

                // Return the wishlist and its items
                return Response.WriteAsJsonAsync(wishlist.ResponseObj(context));
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return Response.WriteAsync("Something went wrong with the server");
            }
        });

        //make lists(create)
        group.MapPost("/", (HttpContext context) => {
            var (Request, Response) = (context.Request, context.Response);

            try {
                Models.User? User = context.Items["User"] as Models.User;

                if (User == null) {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Response.WriteAsync("Only logged-in users can create lists");
                }

                var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();

                // Get the name of the list from the form data
                string? listName = Convert.ToString(Request.Form["name"]).Trim();

                if (string.IsNullOrEmpty(listName)) {
                    Response.StatusCode = StatusCodes.Status406NotAcceptable;
                    return Response.WriteAsync("You need to provide a name for the list");
                }

                // Create a new list (could be a wishlist or any other type of list)
                Models.List newList = new Models.List
                {
                    Name = listName,
                    UserID = User.UserID,
                    CreatedOn = DateTime.Now,
                    LastUpdate = DateTime.Now
                };

                // Add the list to the DB Context
                DB.Lists.Add(newList);
                DB.SaveChanges();

                // Return the created list
                var listResponse = new {
                    List = newList
                };

                return Response.WriteAsJsonAsync(newList.ResponseObj(context));
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return Response.WriteAsync("Something went wrong with the server");
            }
        });


        //Delete list
        return group;
    }
}
