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

                // checking quantity exceeding stock
                bool QuantityExceedFlag = false;
                Cart.ToList().ForEach(async (CartItem) =>
                {
                    Models.Product? Product = DB.Products.FirstOrDefault(Product => Product.ProductID == CartItem.ProductID);

                    if (Product == null)
                    {
                        throw new Exception("Something went wrong in the quantity exceeding checker");
                    }

                    if (CartItem.Quantity > Product.StockQuantity)
                    {
                        QuantityExceedFlag = true;
                        Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
                        await Response.WriteAsync($"your quantity of ${CartItem.Quantity} cannot exceed '{Product.ProductName}' stock");
                    }
                });

                if (QuantityExceedFlag) return;

                // Making a new Order
                Models.Order NewOrder = new Models.Order
                {
                    OrderDate = DateTime.Now,
                    UserID = User.UserID,
                    Tax = new decimal(TaxRate)
                };

                DB.Orders.Add(NewOrder);

                DB.SaveChanges();

                DB.OrderItems.AddRange(Cart.ToList().Select(CartItem =>
                {
                    Models.Product? Product = DB.Products.FirstOrDefault(Product => Product.ProductID == CartItem.ProductID);

                    if (Product == null) throw new Exception("Something went wrong when making Order Items");

<<<<<<< HEAD
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
=======
                    return new OrderItem
                    {
                        ProductID = Product.ProductID,
                        OrderID = NewOrder.OrderID,
                        Quantity = CartItem.Quantity,
                        Price = Product.Price,
                    };
                }));
>>>>>>> master

                DB.SaveChanges();

                // updated product qunatity
                DB.Products.UpdateRange(Cart.ToList().Select(CartItem =>
                {
                    Models.Product? Product = DB.Products.FirstOrDefault(Product => CartItem.ProductID == Product.ProductID);

                    if (Product == null) throw new Exception("Something went wrong when updating the product quantity");

                    Product.StockQuantity -= CartItem.Quantity;

                    return Product;
                }));

                DB.SaveChanges();

                // Removing from cart
                DB.UserCartItems.RemoveRange(Cart.ToList());

                // Saving DB Changes
                DB.SaveChanges();

<<<<<<< HEAD
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return Response.WriteAsJsonAsync("delete");
=======
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
>>>>>>> master
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

<<<<<<< HEAD
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
=======
            if (User == null)
>>>>>>> master
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

                            var OrderSubTotal = OrderItems.ToList().Select(OrderItem => OrderItem.Quantity * OrderItem.Price).Sum();

                            return new
                            {
                                Order.OrderID,
                                Order.OrderDate,
                                SubTotal = OrderSubTotal,
                                Order.Tax,
                                Total = Convert.ToDouble(OrderSubTotal) * ((100 + Routes.Order.TaxRate) / 100)
                            };
                        })
                    );
        });

        return group;
    }
}