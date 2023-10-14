using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models;

public class Order
{
    public int OrderID { get; set; }

    [Required]
    public DateTime? OrderDate { get; set; }

    public User User { get; set; } = null!;
    public int UserID { get; set; }

    public int? DiscountID { get; set; }
    public Discount? Discount { get; set; }
    public decimal Tax { get; set; }
}

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