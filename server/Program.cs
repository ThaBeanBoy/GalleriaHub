using System.Text.RegularExpressions;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Models;
using Routes;
using GalleriaMiddleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Cryptography;
using File = System.IO.File;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
var keys = RSA.Create();
keys.ImportRSAPrivateKey(File.ReadAllBytes("keys"), out _);
// builder.Services.AddDbContext<GalleriaHubDBContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Lite")));
builder.Services.AddSqlite<GalleriaHubDBContext>(builder.Configuration.GetConnectionString("Lite"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication("jwt")
    .AddJwtBearer("jwt", gh => {
        gh.TokenValidationParameters = new TokenValidationParameters() {
            ValidateAudience = false,
            ValidateIssuer = false
        };
        
        gh.Events = new JwtBearerEvents() {
            OnMessageReceived = (querh) => {
                if (querh.Request.Query.ContainsKey("user")) {
                    querh.Token = querh.Request.Query["user"];
                }
                return Task.CompletedTask;
            }
        };

        gh.Configuration = new OpenIdConnectConfiguration() {
            SigningKeys = {
                new RsaSecurityKey(keys)
            }
        };
    })
;
builder.Services.AddAuthorization();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseDatabaseConnectionTest();

app.UseAuthentication();

app.MapGet("/", (HttpContext htcx) => htcx.User.FindFirst("GalleriaHub")?.Value ?? "empty");

app.MapGet("/db-connected", (HttpContext context) => {
    var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
    return Results.Ok(DB != null);
});

app.MapGet("/jwt", (string input) => {
    var handler = new JsonWebTokenHandler();
        var token = handler.CreateToken(new SecurityTokenDescriptor() {
            Issuer = "https://localhost:5000",
            Subject = new ClaimsIdentity(new [] {
                new Claim("Email", input.Email)
            })
        });
        return token;
});

app.MapGroup(Developers.RouterPrefix)
    .DeveloperRoutes();

app.MapGroup(Routes.User.RouterPrefix)
    .UserEndpoints();
    //require Authorization
app.Run();

public class TeamMember {
    // Properties
    private string _name;
    private string _studentNr;
    private string? _initials;

    // Constructors
    public TeamMember(string Name, string StudentNr){
        this._name = Name;
        this._studentNr = StudentNr;
    }

    public TeamMember(string Name, string StudentNr, string Initials): this(Name, StudentNr)  {
        this._initials = Initials;
    }

    // Getters & setters
    public string Name {
        get{
            return this._name;
        }
        set{
            this._name = value;
        }
    }

    public string StudentNr {
        get {
            return this._studentNr;
        }
        set {
            ValidateStudentNr(value);
            this._studentNr = value.Trim();
        }
    }

    public string? Initials {
        get {
            return this._initials;
        }
        set {
            this._initials = value;
        }
    }

    private static void ValidateStudentNr(string StudentNr){
        if(StudentNr.Length != 9){
            throw new InvalidStudentNumber();
        }
    }

    public class InvalidStudentNumber : Exception {
        public InvalidStudentNumber(): base("The student number is invalid"){

        }
    }
}
