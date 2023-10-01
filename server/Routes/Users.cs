using System.Security.Claims;
using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Models;
using Utility;

namespace Routes;

public static class User 
{
    public static string RouterPrefix = "/authentication";

    public static RouteGroupBuilder UserEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/sign-up", async (HttpContext context) => {
            Console.WriteLine("handling sign up");
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            
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

                if(inputs.Any(string.IsNullOrEmpty))
                    throw new NullReferenceException();

                // Checking password matching
                if(Password != ConfirmPassword)
                    throw new PasswordConfirmationException();

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
                await SignUserInAsync(context, NewUser);

                // Sending response
                Response.StatusCode = StatusCodes.Status201Created;
                return Response.WriteAsJsonAsync(NewUser);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.StackTrace);

                Response.StatusCode = StatusCodes.Status400BadRequest;
                return Response.WriteAsync("email, username, password & confirm-password required");
            }
            catch(PasswordConfirmationException e)
            {
                Response.StatusCode = StatusCodes.Status409Conflict;
                return Response.WriteAsync(e.Message);
            }
            catch(EmailAlreadyRegisteredException e)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return Response.WriteAsync(e.Message);
            }
            catch(UsernameAlreadyTakenException e)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return Response.WriteAsync(e.Message);
            }
            catch (Exception)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;  
                return Response.WriteAsync("Something went wrong"); 
            }
        });

        group.MapPost("/login", async (HttpContext context) => {
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();

            try
            {
                string? UserName = Convert.ToString(Request.Form["username"]).Trim();
                string? Password = Convert.ToString(Request.Form["password"]).Trim();

                string?[] info = {UserName,Password};

                //Checking if the received Username and Password is not empty
                if((UserName == null) || (Password == null))
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

                await SignUserInAsync(context, userEntity);

                // Returning user object
                await Response.WriteAsJsonAsync(userEntity);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                await Response.WriteAsync("Login Fail");
            }

        });

        // Add logout route
        group.MapPost("/logout", async (HttpContext context) => {
            await context.SignOutAsync();
            return "logged out";
        });

        return group;
    }

    private static async Task SignUserInAsync(HttpContext context,Models.User UserModel)
    {
        try
        {
            List<Claim> Claims = new(){
                // new Claim(ClaimTypes.NameIdentifier, User.UserID.ToString())
                new Claim(ClaimTypes.NameIdentifier, UserModel.UserID.ToString())
            };
            var Identity = new ClaimsIdentity(Claims, "cookie");
            var User = new ClaimsPrincipal(Identity);
            await context.SignInAsync("cookie", User);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    // Exceptions

    public class PasswordConfirmationException : Exception
    {
        public PasswordConfirmationException(): base("Password did not match the confirmation password"){}
    }

    public class EmailAlreadyRegisteredException : Exception
    {
        public EmailAlreadyRegisteredException(string Email) : base($"{Email} has already been registered"){}
    }

    public class UsernameAlreadyTakenException : Exception
    {
        public UsernameAlreadyTakenException(string Username) : base($"{Username} already taken"){}
    }

    public class UsernameNotFoundException : Exception
    {
        public UsernameNotFoundException(string UserName) : base($"{UserName} doesn't exist"){}
    }
}