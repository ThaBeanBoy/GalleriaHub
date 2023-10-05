using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Galleria.Services;

namespace server.Routes
{
    public static class APIResponse
    {
        
        public static object User(Models.User User)
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

        public static object User(JwtSecurityToken Token)
        {
            return new 
            {
                token = JWTService.TokenString(Token),
                expiryDate = Token.ValidTo
            };
        }

        public static object User(Models.User User, JwtSecurityToken Token)
        {
            return new {
                JWT = APIResponse.User(Token),
                User = APIResponse.User(User)
            };
        }

        public static object User(JwtSecurityToken Token, Models.User User)
        {
            return APIResponse.User(User, Token);
        }
    }
}