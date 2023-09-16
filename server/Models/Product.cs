using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models;

public class Product : IDateTime
{
    public int ProductID { get; set; }

    public int ArtistID { get; set; }
    public Artist Artist { get; set; } = null!;

    public string ProductName {get; set;} = null!; 

    // ! Product typename is still under review
    [Required]
    [Range(
        minimum: 0.01, 
        maximum: 1000000000,
        ErrorMessage = "Price must be between 0.01 & 1000000000" 
    )]
    [Column(TypeName = "decimal(6, 2)")]
    public decimal Price {get; set;}

    [Required]
    public int StockQuantity {get; set;}

    [Required]
    public bool Public {get; set;}

    public DateTime CreatedOn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    
    public DateTime LastUpdate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    // Can be null
    // Should reference file in file storage
    public File? File {get; set;} = null!;
    public int? FileID { get; set; }
}

[PrimaryKey(nameof(ProductID), nameof(FileID))]
public class ProductFile
{
    public Product Product { get; set; } = null!;
    public int ProductID { get; set; }
    public File File { get; set; } = null!;
    public int FileID { get; set; }
    public bool Public { get; set; }
}

[PrimaryKey(nameof(ProductID), nameof(VerifierID))]
public class ProductVerification
{
    public int ProductID { get; set; }
    public Product Product { get; set; } = null!;

    public int VerifierID { get; set; }
    public Verifier Verifier { get; set; } = null!;
}