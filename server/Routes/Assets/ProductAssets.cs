using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace server.Routes.Assets;

public static class ProductAssets
{
    public static string RouterPrefix = "/products";
    
    public static RouteGroupBuilder ProductAssetEndpoints(this RouteGroupBuilder group)
    {
        // (create) upload file
        group.MapPost("/{id}", (HttpContext context) => {
            Console.WriteLine("Uploading");
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            var User = context.Items["User"] as Models.User;

            try
            {
                // Check if user is logged in
                if(User == null)
                {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Response.WriteAsync("Not logged in");
                }

                int ProductID = int.Parse(context.GetRouteValue("id") as string ?? "error");
                Models.Product? Product = DB.Products.FirstOrDefault(Product => Product.ProductID == ProductID);
                
                if(Product == null)
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    return Response.WriteAsync("Product not found");
                }

                // todo: Check if the request user owns the product
                if(Product.UserID != User.UserID)
                {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Response.WriteAsync($"User {User.UserID} doesn't own the product");
                }

                 // todo: get the files from the form
                var Files = context.Request.Form.Files;

                // todo: check for empty files
                if(Files.Count == 0)
                {
                    Response.StatusCode = StatusCodes.Status406NotAcceptable;
                    Response.WriteAsync("No files found");
                }

                /*
                    todo: upload files to S3,
                    todo: Add the path to the db
                */

                // todo: return the updated product
                Response.StatusCode = StatusCodes.Status501NotImplemented;
                return Response.WriteAsync($"Not implemented, files {Files.Count}");
            }
            catch(Exception)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return Response.WriteAsync("Something went wrong");
            }
        });

        // (read) get file
        group.MapGet("/{id}/assets/{asset}", (HttpContext context) => {
            // check if the product is private or the request user owns the product

            // return the asset
        });

        // delete files
        group.MapDelete("/{id}/assets/{asset}", (HttpContext context) => {
            // Check if the request user owns the product

            // delegte the file
        });
        
        return group;
    }
}