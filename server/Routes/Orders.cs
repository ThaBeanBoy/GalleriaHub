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

    public static double TaxRate = 15;

    public static RouteGroupBuilder OrderEndpoints(this RouteGroupBuilder group)
    {
        group.MapPut("/", async (HttpContext context) =>
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

                var Cart = DB.UserCartItems.Where(CartItem => CartItem.UserID == User.UserID);

                if (Cart.ToList().Count == 0)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    await Response.WriteAsync("Cart is empty");
                    return;
                }

                // Making a new Order
                Models.Order NewOrder = new Models.Order
                {
                    OrderDate = DateTime.Now,
                    UserID = User.UserID,
                    Tax = new decimal(Order.TaxRate)
                };

                DB.Orders.Add(NewOrder);
                DB.SaveChanges();

                DB.OrderItems.AddRange(Cart.ToList().Select(CartItem =>
                {
                    Models.Product? Product = DB.Products.FirstOrDefault(Product => Product.ProductID == CartItem.ProductID);

                    return new OrderItem
                    {
                        ProductID = Product.ProductID,
                        OrderID = NewOrder.OrderID,
                        Quantity = CartItem.Quantity,
                        Price = Product.Price,
                    };
                }));

                DB.SaveChanges();

                // Removing from cart
                DB.UserCartItems.RemoveRange(Cart.ToList());

                DB.SaveChanges();

                // Response
                await Response.
                    WriteAsJsonAsync(
                        DB.Orders
                        .Where(Order => Order.UserID == User.UserID)
                        .ToList()
                        .Select(Order =>
                        {
                            var OrderItems = DB.OrderItems.Where(OrderItem => OrderItem.OrderID == Order.OrderID);

                            var OrderTitle = OrderItems.ToList().Select(OrderItem => OrderItem.Quantity * OrderItem.Price).Sum();

                            return new
                            {
                                Order.OrderID,
                                Order.OrderDate,
                                Total = OrderTitle,
                                Tax = Order.Tax,
                            };
                        })
                    );
            }
            catch (Exception)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                await Response.WriteAsync("Something went wrong");
            }

        });

        group.MapGet("/", async (HttpContext context) =>
        {
            var (Request, Response) = (context.Request, context.Response);
            Models.User? User = context.Items["User"] as Models.User;
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();

            if (User == null)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                await Response.WriteAsync("Need to be logged in");
                return;
            }

            await Response.
                    WriteAsJsonAsync(
                        DB.Orders
                        .Where(Order => Order.UserID == User.UserID)
                        .ToList()
                        .Select(Order =>
                        {
                            var OrderItems = DB.OrderItems.Where(OrderItem => OrderItem.OrderID == Order.OrderID);

                            var OrderTitle = OrderItems.ToList().Select(OrderItem => OrderItem.Quantity * OrderItem.Price).Sum();

                            return new
                            {
                                Order.OrderID,
                                Order.OrderDate,
                                Total = OrderTitle,
                                Tax = Order.Tax,
                            };
                        })
                    );
        });

        return group;
    }
}