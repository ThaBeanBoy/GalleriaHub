using System.Text.RegularExpressions;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Models;
using Routes;
using GalleriaMiddleware;

var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddDbContext<GalleriaHubDBContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Lite")));
builder.Services.AddSqlite<GalleriaHubDBContext>(builder.Configuration.GetConnectionString("Lite"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseDatabaseConnectionTest();

app.MapGet("/", ()=> "Hello World");

app.MapGet("/db-connected", (HttpContext context) => {
    var DB = context.RequestServices.GetRequiredService<GalleriaHubDBContext>();
    return Results.Ok(DB != null);
});

app.MapGroup(Developers.RouterPrefix)
    .DeveloperRoutes();

app.MapGroup(Routes.User.RouterPrefix)
    .UserEndpoints();
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
