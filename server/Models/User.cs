using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Models;

/*
https://weblogs.asp.net/manavi/inheritance-mapping-strategies-with-entity-framework-code-first-ctp5-part-1-table-per-hierarchy-tph
*/

public abstract class User : IDateTime 
{
    public int UserID {get; set;}

    public string Email {get; set;} = null!;

    public string Password {get; set;} = null!;

    public string Public { get; set; } = null!;

    public byte[]? ProfilePicture { get; set; } = null!;

    public byte[]? CoverImage { get; set; }

    // Can be null
    [RegularExpression("^[a-z0-9.]$", ErrorMessage = "Only the letters (a-z), numbers (0-9) and dots (.), are allowed")]
    public string? Username {get; set;} = null!;

    // Can be null
    public string? Name {get; set;} = null!;

    public DateTime CreatedOn {get; set;}

    public DateTime LastUpdate { get; set; }
    
    public string? Description {get; set;} = null!;

    // Can be null
    public string? Location {get; set;} = null!;

    public int ListID { get; set; }
    public List WishList { get; set; } = null!;
}

[Table("Verifiers")]
public class Verifier : User
{
    public int GalleryID { get; set; }
    public Gallery Gallery { get; set; } = null!;

    public ICollection<Product> VerifiedArtworks { get; } = new List<Product>();
    public ICollection<Artist> VerifiedArtist { get; } = new List<Artist>();
}

[Table("Artists")]
public class Artist : User
{
    public int VerifierID { get; set; }

    public ICollection<Product> Artworks { get; } = new List<Product>();
}