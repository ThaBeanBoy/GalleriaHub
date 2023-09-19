using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models;

public class Order : IList<OrderItem>
{
    public int OrderID {get; set;}

    [Required]
    public DateTime OrderDate {get; set;}

    [Required, ForeignKey("User")]
    public required User Customer {get; set;}

    public int? DiscountID { get; set; }
    public Discount? Discount { get; set; }

    public void AddItem(DbSet<OrderItem> ListItemSet, OrderItem NewItem)
    {
        throw new NotImplementedException();
    }

    public OrderItem[] GetItems(DbSet<OrderItem> ListItemSet)
    {
        throw new NotImplementedException();
    }

    public void RemoveItem(DbSet<OrderItem> ListItemSet, OrderItem NewItem)
    {
        throw new NotImplementedException();
    }

    public void UpdateOrderItem(DbSet<OrderItem> ListItemSet, OrderItem NewItem)
    {
        throw new NotImplementedException();
    }
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