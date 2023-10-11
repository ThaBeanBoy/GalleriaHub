using System.Reflection.Metadata;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Models;
using Cart;
using System.Reflection;
using Galleria.Middleware;

namespace Routes {

public static class Cart {
    public static string CartRoutePrefix = "/cart";

    private static bool hasPaid;

    public static RouteGroupBuilder CartEndpoints(this RouteGroupBuilder cartGroup) {
        cartGroup.MapPost("/checkout", (HttpContext http) =>
        {
            var (request, response) = (http.Request, http.Response);
            var db = http.RequestServices.GetRequiredService<GalleriaHubDBContext>();

            var token = request.ReadFromJsonAsync<Shipping>();
            try {
            if (token.IsCompletedSuccessfully) {
                Models.User user = token.Result.Customers;
                if (user != null) {
                    dynamic orders = (from o in db.Orders 
                    where (o.OrderDate.Month <= 30) 
                    && o.Customer.Equals(user)
                    select o).DefaultIfEmpty();
                    var products = (from oi in db.OrderItems
                    join p in db.Products on oi.ProductID equals p.ProductID 
                    select new{
                        Name = p.ProductName,
                        TotalPrice = oi.Price,
                        ID = oi.OrderID,
                        Quantity = oi.Quantity
                    }).FirstOrDefault();
                            if ((products != null) && (orders != null) && (products.ID.Equals(orders.OrderID))) {
                                
                                response.StatusCode = StatusCodes.Status200OK;
                                response.WriteAsJsonAsync(products);
                                response.WriteAsJsonAsync(token);
                                hasPaid = true;
                            }
                            else {
                                hasPaid = false;
                            }
                        
                        
                      
                }
            }
            }
            catch (Exception) {
                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.WriteAsync("Something Happened");
            }
        });

        cartGroup.MapGet("/checkout", (HttpContext context) => {
            if (hasPaid) {
                context.Response.WriteAsync("User has paid");
                context.Response.StatusCode = StatusCodes.Status200OK;
            }
            else {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.WriteAsync("User has not paid yet");
            }
        });
        
        return cartGroup;
        }

    

    
}

public class Shipping {
    public required Models.User Customers { get; set; }
    public required string Address1 { get; set; }
    public string? Address2 { get; set; }
    public required string Suburb { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    public required string Code { get; set; }

}



}    
