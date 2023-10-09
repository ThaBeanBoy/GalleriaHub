using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Galleria.Services;
using Models;

namespace server.Routes.Assets;

public static class ProductAssets
{
    public static string RouterPrefix = "/products";

    public static RouteGroupBuilder ProductAssetEndpoints(this RouteGroupBuilder group)
    {
        // (create) upload file
        group.MapPost("/{id}", (HttpContext context) =>
        {
            Console.WriteLine("Uploading");
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            var S3 = context.RequestServices.GetRequiredService<S3BucketService>();

            var User = context.Items["User"] as User;

            try
            {
                // Check if user is logged in
                if (User == null)
                {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Response.WriteAsync("Not logged in");
                }

                int ProductID = int.Parse(context.GetRouteValue("id") as string ?? "error");
                Product? Product = DB.Products.FirstOrDefault(Product => Product.ProductID == ProductID);

                if (Product == null)
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    return Response.WriteAsync("Product not found");
                }

                // todo: Check if the request user owns the product
                if (Product.UserID != User.UserID)
                {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Response.WriteAsync($"User {User.UserID} doesn't own the product");
                }

                // todo: get the files from the form
                IFormFileCollection Files = context.Request.Form.Files;

                // todo: check for empty files
                if (Files == null || Files.Count == 0)
                {
                    Response.StatusCode = StatusCodes.Status406NotAcceptable;
                    return Response.WriteAsync("No files found");
                }

                /*
                    todo: upload files to S3,
                    todo: Add the path to the db
                */
                S3.upload();

                // Save the files in the static folder

                // todo: return the updated product
                Response.StatusCode = StatusCodes.Status501NotImplemented;
                return Response.WriteAsync($"Supposed to upload {Files.Count} files,");
            }
            catch (InvalidOperationException ex)
            {
                Response.StatusCode = StatusCodes.Status406NotAcceptable;
                return Response.WriteAsync(ex.Message);
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine(ex);
                return Response.WriteAsync(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return Response.WriteAsync("Something went wrong");
            }
        });

        group.MapGet("/{file-name}", (HttpContext context, IWebHostEnvironment env) =>
        {
            var (Request, Response) = (context.Request, context.Response);
            var S3 = context.RequestServices.GetRequiredService<S3BucketService>();

            try
            {
                string? key = context.GetRouteValue("file-name") as string;

                // Checking for nulls
                if (key == null)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Response.WriteAsync("Need a file name");
                }

                Response.StatusCode = StatusCodes.Status501NotImplemented;
                return Response.SendFileAsync(S3.download(env, key));
                // return Response.WriteAsync("Still implement");
            }
            catch (Exception ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return Response.WriteAsync(env.IsDevelopment() ? ex.Message : "Something went wrong");
            }
        });

        // (read) get file
        group.MapGet("/{id}/assets/{asset}", (HttpContext context) =>
        {
            // check if the product is private or the request user owns the product

            // return the asset
        });

        // delete files
        group.MapDelete("/{id}/assets/{asset}", (HttpContext context) =>
        {
            // Check if the request user owns the product

            // delegte the file
        });

        return group;
    }
}