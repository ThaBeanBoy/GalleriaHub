using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Models;

/*
https://weblogs.asp.net/manavi/inheritance-mapping-strategies-with-entity-framework-code-first-ctp5-part-1-table-per-hierarchy-tph
*/

public class User : IDateTime
{
    public int UserID { get; set; }

    public string Email { get; set; } = null!;

    [JsonIgnore]
    public string Password { get; set; } = null!;

    public bool Public { get; set; }

    // Can be null
    [RegularExpression("^[a-z0-9.]$", ErrorMessage = "Only the letters (a-z), numbers (0-9) and dots (.), are allowed")]
    public string? Username { get; set; } = null!;

    // Can be null
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? PhoneNumber { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime LastUpdate { get; set; }

    public int DescriptionFileID { get; set; }

    // Can be null
    public string? Location { get; set; } = null!;

    [ForeignKey("ProfilePicture")]
    public int? ProfilePictureFileID { get; set; }
    public File? ProfilePicture { get; set; } = null!;

    [ForeignKey("CoverPicture")]
    public int? CoverPictureFileID { get; set; }

    public File? CoverPicture { get; set; }
}

public class Verifier
{
    public int VerifierID { get; set; }
    public int UserID { get; set; }
    public User User { get; set; } = null!;

    public int GalleryID { get; set; }
    public Gallery Gallery { get; set; } = null!;

    // ARTIST VERIFICATION CRUD
    // CREATE - verify new artist
    public void VerifyUser(DbSet<ArtistVerification> verifications, User User)
    {
        throw new NotImplementedException();
    }

    // RETRIEVE - get verified artists
    public User[] GetVerifiedArtists(DbSet<ArtistVerification> verifications)
    {
        throw new NotImplementedException();
    }

    // DELETE - Remove verification
    public void RemoveArtistVerification(DbSet<ArtistVerification> verifications, User Artist)
    {
        throw new NotImplementedException();
    }

    // PRODUCT VERIFICATIONS CRUD
    // CREATE - verify artwork
    public void VerifyProduct(DbSet<ProductVerification> verifications, Product Product)
    {
        throw new NotImplementedException();
    }

    //RETRIEVE - get verified artworks
    public void GetVerifiedProducts(DbSet<ProductVerification> verifications)
    {
        throw new NotImplementedException();
    }
}

[PrimaryKey(nameof(UserID), nameof(VerifierID))]
public class ArtistVerification
{
    public int UserID { get; set; }
    public User User { get; set; } = null!;

    public int VerifierID { get; set; }
    public Verifier Verifier { get; set; } = null!;
}