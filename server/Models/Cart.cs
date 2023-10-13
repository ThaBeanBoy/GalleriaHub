using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace Models;

public class CartItem
{
    public User User { get; set; } = null!;
    public int UserID { get; set; }
    public int ProductID { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
}