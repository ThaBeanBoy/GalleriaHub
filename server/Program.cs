using System.Text.RegularExpressions;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Models;
using Routes;
using GalleriaMiddleware;
using Microsoft.AspNetCore.Cors;
using server.Middleware;
using System.Text;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddDbContext<GalleriaHubDBContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Lite")));
builder.Services.AddSqlite<GalleriaHubDBContext>(builder.Configuration.GetConnectionString("Lite"));

// CORS
string ClientOrigins = "_ClientOrigins";
builder.Services.AddCors(options =>{
    options.AddPolicy( 
        name: ClientOrigins, 
        policy => {
            policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();            
        });
});

string DevelopmentCORS = "_DevModeCors";
builder.Services.AddCors(options => {
    options.AddPolicy(
        name: DevelopmentCORS ,
        policy => {
            policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        }
    );
});

// builder.Services.AddAuthentication("cookie")
//     .AddCookie("cookie");

// Configuring JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["key"]);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters{
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(app.Environment.IsProduction() ? ClientOrigins : DevelopmentCORS);

app.UseAuthentication();
// app.UseUserMiddleware();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", ()=> "Hello World");

app.MapGet("/db-connected", (HttpContext context) => {
    var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
    return Results.Ok(DB != null);
});

app.MapGroup(Developers.RouterPrefix)
    .DeveloperRoutes();//.WithMetadata(new EnableCorsAttribute(ClientOrigins));

app.MapGroup(Routes.User.RouterPrefix)
    .UserEndpoints(jwtSettings);//.WithMetadata(new EnableCorsAttribute(ClientOrigins));
    // .RequireAuthorization();

app.MapGroup(Routes.Product.RouterPrefix)
    .ProductEndpoints();

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
