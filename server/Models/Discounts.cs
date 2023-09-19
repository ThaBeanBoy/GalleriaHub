using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Discount
{
    public int DiscountID { get; set; }

    public string DiscountCode { get; set; } = null!;

    [Range(
        minimum: 0.01, 
        maximum: 1000000000,
        ErrorMessage = "Price must be between 0.01 & 1000000000" 
    )]
    [Column(TypeName = "decimal(6, 2)")]
    public decimal DiscountPercentage { get; set; }

    public bool DiscountUsed { get; set; }

    public static string GeneratedCode()
    {
        throw new NotImplementedException();
    }
}