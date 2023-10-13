using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;
using Microsoft.EntityFrameworkCore;

namespace Models;

public class Product : IDateTime
{
    public int ProductID { get; set; }

    public int UserID { get; set; }
    public User User { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    // ! Product typename is still under review
    [Required]
    [Range(
        minimum: 0.01,
        maximum: 1000000000,
        ErrorMessage = "Price must be between 0.01 & 1000000000"
    )]
    [Column(TypeName = "decimal(6, 2)")]
    public decimal Price { get; set; }

    [Required]
    public int StockQuantity { get; set; }

    [Required]
    public bool Public { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime LastUpdate { get; set; }

    public string Description { get; set; } = null!;
}

public class ProductFile
{
    public int ProductFileID { get; set; }

    public Product Product { get; set; } = null!;
    public int ProductID { get; set; }

    public string FileKey { get; set; } = null!;

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