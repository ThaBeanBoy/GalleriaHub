using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Galleria.Services;
using Models;

namespace server.Routes;

public static class APIResponse
{
    // USER Response objects
    public static object ResponseObj(this User User)
    {
        return new
        {
            userID = User.UserID,
            email = User.Email,
            username = User.Username,
            createdOn = User.CreatedOn,
            lastUpdate = User.LastUpdate,
            profilePicture = User.ProfilePictureFileID,
            name = User.Name,
            surname = User.Surname,
            phoneNumber = User.PhoneNumber,
            location = User.Location
        };
    }

    public static object ResponseObj(this JwtSecurityToken Token)
    {
        return new
        {
            token = JWTService.TokenString(Token),
            expiryDate = Token.ValidTo
        };
    }

    public static object ResponseObj(this User User, JwtSecurityToken Token)
    {
        return new
        {
            JWT = Token.ResponseObj(),
            User = User.ResponseObj()
        };
    }

    // Product response
    public static object ResponseObj(this Product Product, HttpContext context)
    {
        var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
        var Request = context.Request;

        User? User = context.Items["User"] as User;

        // Getting the user
        User? ProductOwner = DB.Users.FirstOrDefault(User => Product.UserID == User.UserID);
        // ProductFile Description = DB.ProductFiles.FirstOrDefault(PD => PD.FileID == Product.FileID);

        var Images = DB
            .ProductFiles
            .Where(PF => PF.ProductID == Product.ProductID)

            // Filtering privacy based on publicity or owner
            // No time to implement
            // .Where(PF => PF.Public || (User != null && Product.UserID == User.UserID))

            // Transforming into useable endpoints
            .Select(PF => $"{Request.Scheme}://{Request.Host}/assets/products/{Product.ProductID}/{PF.FileKey}");

        return new
        {
            productID = Product.ProductID,
            productName = Product.ProductName,
            price = Product.Price,
            stockQuantity = Product.StockQuantity,
            Product.Public,
            createdOn = Product.CreatedOn,
            lastUpdate = Product.LastUpdate,
            Description = Product.Description,
            seller = ProductOwner.ResponseObj(),
            Images,
        };
    }

    // Product File
    public static string ResponseObj(this ProductFile File)
    {
        return $"/assets/products/{File.FileKey}";
    }

    // List
    public static object ResponseObj(this Models.List List, HttpContext context){
        var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();

        List<Models.ListItem> listItems = DB.ListItems.Where(li => li.ListID == List.ListID).ToList();
        
        return new {
                    ListID = List.ListID,
                    Name = List.Name,
                    CreatedOn = List.CreatedOn,
                    LastUpdate = List.LastUpdate,
                    Items = listItems.Select(Item => 
                        DB.Products
                        .FirstOrDefault(Product => Product.ProductID == Item.ProductID)
                        .ResponseObj(context)
                        )
                };
    }
}