using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Models;
using server.Routes;

namespace Routes;

public static class Order
{
    public static string RouterPrefix = "/orders";

    public static RouteGroupBuilder OrderEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/{ProductID}", (HttpContext context) =>
        {
            var (Request, Response) = (context.Request, context.Response);
            Models.User? User = context.Items["User"] as Models.User;
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();

            try
            {
                // Checking if user is logged in
                if (User == null)
                {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Response.WriteAsync("Need to be logged in");
                }

                // Getting the product ID
                int productId = int.Parse(context.GetRouteValue("productId") as string ?? "0");
                Models.Product? Product = DB.Products.FirstOrDefault(Product => Product.ProductID == productId);

                if (Product == null)
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    return Response.WriteAsync("Product not found");
                }

                // looking for the Order in the DB
                Models.Order? Order = DB.Orders.FirstOrDefault(Order => Order.UserID == User.UserID && Order.Pending);

                // Creating the Order if it doesn't exist
                if (Order == null)
                {
                    Models.Order NewOrder = new Models.Order
                    {
                        UserID = User.UserID,
                        OrderDate = DateTime.Now,
                        Pending = true
                    };
                    DB.Orders.Add(NewOrder);

                    DB.SaveChanges();
                    Order = NewOrder;
                }

                try
                {
                    DB.OrderItems.Add(new OrderItem
                    {
                        ProductID = Product.ProductID,
                        OrderID = Order.OrderID,
                        Quantity = 1,
                        Price = Product.Price,
                    });

                    DB.SaveChanges();
                }
                catch (Exception)
                {
                    // Do nothing
                }


                Response.StatusCode = StatusCodes.Status202Accepted;
                return Response.WriteAsJsonAsync("Added to order");
            }
            catch (FormatException ex)
            {
                Response.StatusCode = StatusCodes.Status406NotAcceptable;
                return Response.WriteAsync(ex.Message);
            }
            catch (Exception)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return Response.WriteAsync("Something went wrong");
            }
        });

        group.MapDelete("/{ProductID}", (HttpContext context) =>
        {
            var (Request, Response) = (context.Request, context.Response);
            Models.User? User = context.Items["User"] as Models.User;
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();

            try
            {
                if (User == null)
                {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Response.WriteAsync("Need to be logged in");
                }

                // Getting the product ID
                int productId = int.Parse(context.GetRouteValue("productId") as string ?? "0");
                Models.Product? Product = DB.Products.FirstOrDefault(Product => Product.ProductID == productId);

                if (Product == null)
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    return Response.WriteAsync("Product not found");
                }

                Models.Order? Order = DB.Orders.FirstOrDefault(Order => Order.UserID == User.UserID && Order.Pending);
                if (Order == null) throw new Exception();

                OrderItem? OrderItemToBeDeleteted = DB.OrderItems.FirstOrDefault(OrderItem => OrderItem.OrderID == Order.OrderID && OrderItem.ProductID == Product.ProductID);
                if (OrderItemToBeDeleteted == null) throw new Exception();

                DB.OrderItems.Remove(OrderItemToBeDeleteted);
                DB.SaveChanges();

                Response.StatusCode = StatusCodes.Status400BadRequest;
                return Response.WriteAsJsonAsync("delete");
            }
            catch (Exception)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return Response.WriteAsync("Something went wrong");
            }
        });

        group.MapPut("/{ProductID}", async (HttpContext context) =>
        {
            var (Request, Response) = (context.Request, context.Response);
            Models.User? User = context.Items["User"] as Models.User;
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();

            try
            {
                if (User == null)
                {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await Response.WriteAsync("Need to be logged in");
                    return;
                }

                int productId = int.Parse(context.GetRouteValue("productId") as string ?? "0");
                var Product = DB.Products.FirstOrDefault(Product => Product.ProductID == productId);

                if (Product == null)
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    await Response.WriteAsync("Couldnt find product");
                    return;
                }

                var Order = DB.Orders.FirstOrDefault(Order => Order.UserID == User.UserID && Order.Pending);

                var OrderItemToBeUpdated = DB.OrderItems.FirstOrDefault(OrderItem => OrderItem.OrderID == Order.OrderID && OrderItem.ProductID == Product.ProductID);

                int newVal = int.Parse(Request.Query["value"]);

                if (newVal < 1)
                {
                    Response.StatusCode = StatusCodes.Status406NotAcceptable;
                    await Response.WriteAsync("quantity value cannot be less than 1");
                    return;
                }

                OrderItemToBeUpdated.Quantity = newVal;

                DB.OrderItems.Update(OrderItemToBeUpdated);

                DB.SaveChanges();

                await Response.WriteAsJsonAsync("added to order");
            }
            catch (ArgumentNullException)
            {

                Response.StatusCode = StatusCodes.Status400BadRequest;
                await Response.WriteAsync("SOmething is wrong with the input");
            }
            catch (FormatException)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                await Response.WriteAsync("SOmething is wrong with the input");
            }
            catch (Exception)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                await Response.WriteAsync("Something went wrong");
            }

        });

        return group;
    }
}