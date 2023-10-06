using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

using Models;
using Utility;
using Galleria.Services;
using server.Routes;

using static server.Routes.APIResponse;

namespace Routes;

public static class User 
{
    public static string RouterPrefix = "/authentication";

    public static RouteGroupBuilder UserEndpoints(this RouteGroupBuilder group, IConfigurationSection configuration)
    {
        // sign up
        group.MapPost("/sign-up", async (HttpContext context) => {
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            var JWT = context.RequestServices.GetRequiredService<JWTService>();

            Response.StatusCode = StatusCodes.Status501NotImplemented;

            try
            {
                // Getting the form content
                string? Email = Convert.ToString(Request.Form["email"]).Trim();
                string? UserName = Convert.ToString(Request.Form["username"]).Trim();
                string? Password = Convert.ToString(Request.Form["password"]).Trim();
                string? ConfirmPassword = Convert.ToString(Request.Form["confirm-password"]).Trim();

                // Checking for any nulls in form
                string?[] inputs = {Email, UserName, Password, ConfirmPassword};

                Console.WriteLine(inputs);

                if(inputs.Any(string.IsNullOrEmpty)){
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    await Response.WriteAsync("email, username, password & confirm-password required");
                    return;
                }

                // Checking password matching
                if(Password != ConfirmPassword)
                {
                    Response.StatusCode = StatusCodes.Status409Conflict;
                    await Response.WriteAsync("Password did not match the confirmation password");
                    return;
                }

                // Checking if email is registered
                if(DB.Users.Any(U => U.Email == Email))
                    throw new EmailAlreadyRegisteredException(Email);

                // Checking username uniqueness
                if(DB.Users.Any(U => U.Username == UserName))
                    throw new UsernameAlreadyTakenException(UserName);

                // Attempting save in DB
                List NewUserWishList = new List(); 
                DB.Lists.Add(NewUserWishList);

                Models.User NewUser = new Models.User(){
                    Email = Email,
                    Password = Security.hashauth(Password), // Encrypting
                    Username = UserName,
                    Public = true,
                    CreatedOn = DateTime.Now,
                    LastUpdate = DateTime.Now,
                    WishList = NewUserWishList
                };
                DB.Users.Add(NewUser);

                DB.SaveChanges();

                // Signing user in
                // await SignUserInAsync(context, NewUser);
                var Token = JWT.GenerateToken(NewUser);
                // Sending response
                Response.StatusCode = StatusCodes.Status201Created;
                await Response.WriteAsJsonAsync(NewUser.ResponseObj());
            }
            catch(EmailAlreadyRegisteredException e)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                await Response.WriteAsync(e.Message);
            }
            catch(UsernameAlreadyTakenException e)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                await Response.WriteAsync(e.Message);
            }
            catch (Exception)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;  
                await Response.WriteAsync("Something went wrong"); 
            }
        });

        // login
        group.MapPost("/login", async (HttpContext context) => {
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            var JWT = context.RequestServices.GetRequiredService<JWTService>();

            try
            {
                string? UserName = Convert.ToString(Request.Form["username"]).Trim();
                string? Password = Convert.ToString(Request.Form["password"]).Trim();

                // Console.WriteLine($"Username / Email : {UserName}\nPassword: {Password}");

                string?[] inputs = {UserName,Password};
                //Checking if the received Username and Password is not empty
                if(inputs.Any(string.IsNullOrEmpty))
                {
                    Response.StatusCode = StatusCodes.Status406NotAcceptable;
                    await Response.WriteAsync("Can not accept empty field. Provide username and password");
                    return;
                }
                
                //Getting the user based on the username
                Models.User? userEntity = DB.Users.FirstOrDefault(user => user.Username == UserName || user.Email == UserName);

                if(userEntity == null || Security.hashauth(Password) != userEntity.Password)
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    await Response.WriteAsync("Username/Email and/or Password is incorrect");
                    return;
                }

                // Returning user object (succesful login)
                var Token = JWT.GenerateToken(userEntity);
                await Response.WriteAsJsonAsync(
                    userEntity.ResponseObj(Token)
                );
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                await Response.WriteAsync("Login Fail");
            }

        });

        // gets user based on jwt token
        group.MapGet("/get-user", (HttpContext context) => {
            var (Request, Response) = (context.Request, context.Response);

            Models.User? User = context.Items["User"] as Models.User;

            if(User == null){
                Response.StatusCode = StatusCodes.Status404NotFound;
                return Response.WriteAsync("Not logged in");
            }

            return Response.WriteAsJsonAsync(User.ResponseObj());
        });

        // get user on id
        group.MapGet("/{id}", (HttpContext context) => {
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            var User = context.Items["User"] as Models.User;

            try {
                // Get the user ID from the route and convert it to an integer using int.Parse()
                int userId = int.Parse(context.GetRouteValue("id") as string ?? "error");

                // Check if the user with the specified ID exists in the database
                Models.User? userEntity = DB.Users.FirstOrDefault(user => user.UserID == userId);

                // If the user is not found, return a 404 response to the client
                if (userEntity == null) {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    return Response.WriteAsync("User not found");
                }

                // If the user profile is set to private (not public) and the request user is not the same as the requested user, return an error
                if (!userEntity.Public && (User == null || User.UserID != userEntity.UserID)) {
                    Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Response.WriteAsync("User profile is private");
                }

                // Return the user object to the client using the `ResponseObj` method
                return Response.WriteAsJsonAsync(userEntity.ResponseObj());
            }
            catch (FormatException) {
                // Handle the case where ID conversion throws an exception
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return Response.WriteAsync("Invalid user ID format");
            }
            catch (Exception) {
                // Handle other exceptions and return a 500 response
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return Response.WriteAsync("Something went wrong");
            }
        });

        return group;
    }

    public class EmailAlreadyRegisteredException : Exception
    {
        public EmailAlreadyRegisteredException(string Email) : base($"email {Email} has already been registered"){}
    }

    public class UsernameAlreadyTakenException : Exception
    {
        public UsernameAlreadyTakenException(string Username) : base($"username {Username} already taken"){}
    }

    public class UsernameNotFoundException : Exception
    {
        public UsernameNotFoundException(string UserName) : base($"{UserName} doesn't exist"){}
    }
}