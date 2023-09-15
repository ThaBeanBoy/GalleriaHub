using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models;

[PrimaryKey(nameof(OrderID), nameof(ProductID))]
public class OrderItem 
{
    public int ProductID { get; set; }
    public Product Product { get; set; } = null!;

    public int OrderID { get; set; }
    public Order Order { get; set; } = null!;

    [Required]
    public int Quantity { get; set; }

    [Required, Column(TypeName = "decimal(6, 2)")]
    public decimal Price { get; set; }
}