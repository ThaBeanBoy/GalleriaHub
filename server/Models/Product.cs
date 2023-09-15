using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Product : IDateTime
{
    public int ProductID {get; set;}

    [Required]
    public required string ProductName {get; set;} 

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
    public string? Description {get; set;}

    // references to files in file storage
    public ICollection<ProductImage>? Media {get; set;}
}