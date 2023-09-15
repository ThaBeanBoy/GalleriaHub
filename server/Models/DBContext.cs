using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Models;

public class GalleriaHubDBContext : DbContext 
{
    private readonly IConfiguration Configuration;
    // private readonly bool ProductionEnv;

    public GalleriaHubDBContext(IConfiguration Configuration)
    {
        this.Configuration = Configuration;
    }

    // public UsersSet Users { get; set; } = null!;
    public UsersSet Users { get; set; } = null!;
    public DbSet<Verifier> Verifiers { get; set; } = null!;
    public DbSet<Artist> Artists { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
    public DbSet<Gallery> Galleries { get; set; } = null!;
    public DbSet<ProductImage> ProductImages { get; set; } = null!;
    public DbSet<List> Lists { get; set; } = null!;
    public DbSet<ListItem> ListItems { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options){
        // options.UseSqlServer(Configuration.GetConnectionString(
        //     ProductionEnv ? 
        //     "AZURE_SQL_DB" : 
        //     "Local"
        // ));
        options.UseSqlite(Configuration.GetConnectionString("Lite"));
    }

    protected override void OnModelCreating(ModelBuilder builder){
        // Making user emails unique
        builder.Entity<User>()
            .HasIndex(U => U.Email)
            .IsUnique();

        // Making usernames unique
        builder.Entity<User>()
            .HasIndex(U => U.Username)
            .IsUnique();

        // Making the WishList a foreign key to List
        builder.Entity<User>()
            .HasOne(U => U.WishList)
            .WithMany()
            .HasForeignKey(U => U.ListID);

        // Making Artist.UserID both a foreign key & primary key
        builder.Entity<Artist>()
            .HasKey(A => A.UserID);

        builder.Entity<Artist>()
            .HasOne(A => A.User)
            .WithOne()
            .HasForeignKey<Artist>(A => A.UserID);

        // Making Verifier.UserID both a foreign key & primary key
        builder.Entity<Verifier>()
            .HasKey(V => V.UserID);

        builder.Entity<Verifier>()
            .HasOne(V => V.User)
            .WithOne()
            .HasForeignKey<Verifier>(V => V.UserID);
    }

    public class UsersSet : DbSet<User>
    {
        public override IEntityType EntityType => this.EntityType;

        public bool EmailIsRegistered(string Email)
        {
            User U = this.Single(U => U.Email == Email);
            return U != null;
        }

        public bool UsernameAvailable(string Username)
        {
            User U = this.Single(U => U.Username == Username);
            return U == null;
        }
    }
}