using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using server.Routes;

namespace Routes;

public static class Sales
{
    public static string RouterPrefix = "/sales";

    public static RouteGroupBuilder SalesEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", (HttpContext context) =>
        {
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            var User = context.Items["User"] as Models.User;

            try
            {
                if (User == null)
                {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Response.WriteAsync("Need to log in");
                }

                // Getting every product owned by user

                // Getting order items
                var OrderItems = DB.OrderItems.Where(OrderItem => OrderItem.Product.UserID == User.UserID);

                return Response.WriteAsJsonAsync(OrderItems.ToList().Select(OrderItem =>
                {
                    // Getting order
                    Models.Order? Order = DB.Orders.FirstOrDefault(Order => Order.OrderID == OrderItem.OrderID);
                    Models.Product? Product = DB.Products.FirstOrDefault(Product => Product.ProductID == OrderItem.ProductID);
                    string DisplayImage = DB.ProductFiles.FirstOrDefault(PF => PF.ProductID == Product.ProductID).FileKey;

                    return new
                    {
                        Order.OrderID,
                        Order.OrderDate,

                        OrderItem.Quantity,

                        product = new
                        {
                            Product.ProductID,
                            Product.ProductName,
                            OrderItem.Price,
                            DisplayImage
                        },

                        buyer = new
                        {
                            Order.User.UserID,
                            Order.User.Email,
                            Order.User.Username,

                            Order.User.Public,
                            Order.User.CreatedOn,
                            Order.User.LastUpdate
                        },
                    };
                }));
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