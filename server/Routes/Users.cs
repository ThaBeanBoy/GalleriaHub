using Microsoft.IdentityModel.Tokens;
using Models;

namespace Routes;

public static class User 
{
    public static string RouterPrefix = "/authentication";

    public static RouteGroupBuilder UserEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/sign-up", (HttpContext context) => {
            var (Request, Response) = (context.Request, context.Response);
            var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
            
            try
            {
                // Getting the form content
                string? Email = Convert.ToString(Request.Form["email"]).Trim();
                string? UserName = Convert.ToString(Request.Form["username"]).Trim();
                string? Password = Convert.ToString(Request.Form["password"]).Trim();
                string? ConfirmPassword = Convert.ToString(Request.Form["confirm-password"]).Trim();

                // Checking for any nulls in form
                string?[] inputs = {Email, UserName, Password, ConfirmPassword};
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
                    Password = Password, // Supposed to encrypt
                    Username = UserName,
                    Public = true,
                    CreatedOn = DateTime.Now,
                    LastUpdate = DateTime.Now,
                    WishList = NewUserWishList
                };
                DB.Users.Add(NewUser);

                DB.SaveChanges();

                // Send Response & JWT Token

                // Sending response
                Response.StatusCode = StatusCodes.Status201Created;
                return Response.WriteAsync("Successful login");
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

        group.MapPost("/login", (HttpContext context) => {
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
                    throw new NullReferenceException();
                }

                //Getting the user based on the username
                Models.User? userEntity = DB.Users.FirstOrDefault(user => user.Username == UserName);

                if(userEntity == null)
                {
                    throw new UsernameNotFoundException(UserName);
                }

                if(userEntity.Password != Password)
                {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Response.WriteAsync("Incorrect Password");
                }

                Response.StatusCode = StatusCodes.Status200OK;
                return Response.WriteAsync("Access Granted");

            }
            catch(NullReferenceException)
            {
                Response.StatusCode = StatusCodes.Status406NotAcceptable;
                return Response.WriteAsync("Can not accept empty field. Provide username and password");
            }
            catch(UsernameNotFoundException e)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return Response.WriteAsync(e.Message);
            }
            catch(Exception)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return Response.WriteAsync("Login Fail");
            }

        });

        return group;
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
        public UsernameNotFoundException(string UserName) : base($"Could not log in {UserName}")
        {

        }
    }
}