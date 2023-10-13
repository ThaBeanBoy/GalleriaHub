using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Models;

public class GalleriaHubDBContext : DbContext
{
    // private readonly bool ProductionEnv;

    public GalleriaHubDBContext(DbContextOptions options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Verifier> Verifiers { get; set; } = null!;
    public DbSet<ArtistVerification> ArtistVerifications { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<ProductFile> ProductFiles { get; set; } = null!;
    public DbSet<UserCartItem> UserCartItems { get; set; }
    public DbSet<ProductVerification> ProductVerifications { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Discount> Discounts { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
    public DbSet<Gallery> Galleries { get; set; } = null!;
    public DbSet<GalleryFile> GalleryFiles { get; set; } = null!;
    public DbSet<List> Lists { get; set; } = null!;
    public DbSet<ListItem> ListItems { get; set; } = null!;
    public DbSet<File> Files { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        /*
        User Table

        Making user emails unique
        Making usernames unique
        Making the WishList a foreign key to List
        Phone number is unique
        */
        builder.Entity<User>()
            .HasIndex(U => U.Email)
            .IsUnique();

        builder.Entity<User>()
            .HasIndex(U => U.Username)
            .IsUnique();

        builder.Entity<User>()
            .HasIndex(U => U.PhoneNumber)
            .IsUnique();

        /*
        Verifier Table
        
        UserID both a alternate key & primary key
        */
        builder.Entity<Verifier>()
            .HasAlternateKey(V => V.UserID);

        builder.Entity<Verifier>()
            .HasOne(V => V.User)
            .WithOne()
            .HasForeignKey<Verifier>(V => V.UserID);


        // making user images foreign
        builder.Entity<User>()
            .HasOne(u => u.ProfilePicture)
            .WithMany() // Assuming File has no navigation property back to User
            .HasForeignKey(u => u.ProfilePictureFileID);
        // .OnDelete(DeleteBehavior.Restrict);

        // builder.Entity<User>()
        //     .HasOne(u => u.CoverPicture)
        //     .WithMany() // Assuming File has no navigation property back to User
        //     .HasForeignKey(u => u.CoverPictureFileID);
        // .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<User>()
            .HasOne<File>(U => U.CoverPicture)
            .WithOne()
            .HasForeignKey<File>(F => F.FileID);
    }
}