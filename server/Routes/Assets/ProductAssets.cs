using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Galleria.Services;
using Microsoft.EntityFrameworkCore;
using Models;

using static server.Routes.APIResponse;

namespace server.Routes.Assets;

public static class ProductAssets
{
    public static string RouterPrefix = "/products";

    public static RouteGroupBuilder ProductAssetEndpoints(this RouteGroupBuilder group)
    {
        // (create) upload file
        group.MapPost("/{id}", async (HttpContext context, IWebHostEnvironment env) =>
        {
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
                    await Response.WriteAsync("Not logged in");
                    return;
                }

                int ProductID = int.Parse(context.GetRouteValue("id") as string ?? "error");
                Product? Product = DB.Products.FirstOrDefault(Product => Product.ProductID == ProductID);

                if (Product == null)
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    await Response.WriteAsync("Product not found");
                    return;
                }

                // todo: Check if the request user owns the product
                if (Product.UserID != User.UserID)
                {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await Response.WriteAsync($"User {User.UserID} doesn't own the product");
                    return;
                }

                // todo: get the files from the form
                IFormFileCollection Files = context.Request.Form.Files;

                // todo: check for empty files
                if (Files == null || Files.Count == 0)
                {
                    Response.StatusCode = StatusCodes.Status406NotAcceptable;
                    await Response.WriteAsync("No files found");
                    return;
                }

                // uploading files
                foreach (IFormFile File in Files)
                {
                    var file = await S3.Upload(env, File);

                    // Adding to file table
                    ProductFile NewFile = new ProductFile
                    {
                        Product = Product,
                        FileKey = file.Name
                    };

                    // Adding the file to the database
                    DB.ProductFiles.Add(NewFile);

                }

                // updating the lastupdate field
                Product.LastUpdate = DateTime.Now;
                DB.Products.Update(Product);

                // Saving changes to the DB
                DB.SaveChanges();

                // todo: return the updated product
                await Response.WriteAsJsonAsync(Product.ResponseObj(context));
                return;
            }
            catch (InvalidOperationException ex)
            {
                Response.StatusCode = StatusCodes.Status406NotAcceptable;
                await Response.WriteAsync(ex.Message);
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine(ex);
                await Response.WriteAsync(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                await Response.WriteAsync("Something went wrong");
            }
        });

        // (read) get file
        group.MapGet("/{id}/{asset}", (HttpContext context, IWebHostEnvironment env) =>
        {
            var (Request, Response) = (context.Request, context.Response);
            var S3 = context.RequestServices.GetRequiredService<S3BucketService>();

            try
            {
                string? key = context.GetRouteValue("asset") as string;

                // Checking for nulls
                if (key == null)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Response.WriteAsync("Need a file name");
                }

                return Response.SendFileAsync(S3.Download(env, key));
                // return Response.WriteAsync("Still implement");
            }
            catch (Exception ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return Response.WriteAsync(env.IsDevelopment() ? ex.Message : "Something went wrong");
            }
        });

        // delete files
        group.MapDelete("/{id}/{asset}", (HttpContext context, IWebHostEnvironment env) =>
        {
            // Check if the request user owns the product
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            var S3 = context.RequestServices.GetRequiredService<S3BucketService>();
            var User = context.Items["User"] as User;

            try
            {
                int ProductID = int.Parse(context.GetRouteValue("id") as string ?? "error");

                string? key = context.GetRouteValue("asset") as string;

                if (User == null)
                {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    Response.WriteAsync("Need to be logged in to delete asset");
                }

                if (key == null)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Response.WriteAsync("need a file name");
                }

                Product? Product = DB.Products.FirstOrDefault(Product => Product.ProductID == ProductID);

                if (Product == null)
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    return Response.WriteAsync("Product not found");
                }

                if (Product.UserID != User.UserID)
                {
                    Response.StatusCode = StatusCodes.Status403Forbidden;
                    Response.WriteAsync("Only owners of the product can delete the asset.");
                }

                // Deleting the actual file
                S3.Delete(env, key);

                // Deleting from the DB
                DB.ProductFiles.Remove(DB.ProductFiles.FirstOrDefault(PF => PF.FileKey == key));
                DB.SaveChanges();

                return Response.WriteAsJsonAsync(Product.ResponseObj(context));
            }
            catch (FormatException)
            {
                Response.StatusCode = StatusCodes.Status406NotAcceptable;
                return Response.WriteAsync("Product ID no identified");
            }
            catch (AmazonS3Exception ex)
            {
                return Response.WriteAsync(ex.Message);
            }
            catch (Exception)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return Response.WriteAsync("Something went wrong");
            }

        });

        return group;
    }
}