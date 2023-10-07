using System.Text.RegularExpressions;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Models;
using Routes;
using Microsoft.AspNetCore.Cors;
using Galleria.Middleware;
using System.Text;
using Microsoft.IdentityModel.Tokens;

using Galleria.Services;

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

// Adding custom JWT Service
builder.Services.AddGalleriaJWT(jwtSettings);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(app.Environment.IsProduction() ? ClientOrigins : DevelopmentCORS);

app.UseAuthentication();
app.UseUserMiddleware();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", ()=> "Hello World");

app.MapGet("/db-connected", (HttpContext context) => {
    var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
    return Results.Ok(DB != null);
});

app.MapPut("/put-expriment", async (HttpContext context) => {
    var (Request, Response) = (context.Request, context.Response);

    // var Body = await Request.ReadFromJsonAsync<PutModel>();

    await Response.WriteAsJsonAsync(Request.Form);
});

app.MapPost("/upload-img", (HttpContext context)=> {
    var Files = context.Request.Form.Files;

    context.Response.WriteAsync($"Uploaded {Files.Count}");
});

app.MapGroup(Developers.RouterPrefix)
    .DeveloperRoutes();//.WithMetadata(new EnableCorsAttribute(ClientOrigins));

app.MapGroup(Routes.User.RouterPrefix)
    .UserEndpoints(jwtSettings);//.WithMetadata(new EnableCorsAttribute(ClientOrigins));
    // .RequireAuthorization();

app.MapGroup(Routes.Product.RouterPrefix)
    .ProductEndpoints();

app.Run();

class PutModel
{
    public string? Name { get; set; }
    public string? Age { get; set; }
    public IFormFile[]? File {get; set; }
}