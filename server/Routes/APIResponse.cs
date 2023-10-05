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
    public static object ResponseObj(this Product Product, GalleriaHubDBContext DB)
    {
        // Getting the user
        User ProductOwner = DB.Users.FirstOrDefault(User => Product.UserID == User.UserID);
        ProductFile Description = DB.ProductFiles.FirstOrDefault(PD => PD.FileID == Product.FileID);

        return new {
            productID = Product.ProductID,
            productName = Product.ProductName,
            price = Product.Price,
            stockQuantity = Product.StockQuantity,
            Public = Product.Public,
            createdOn = Product.CreatedOn,
            lastUpdate = Product.LastUpdate,
            Description = Description,
            seller = ProductOwner.ResponseObj()
        };
    }
}