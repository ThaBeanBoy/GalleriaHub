using Microsoft.EntityFrameworkCore;

namespace Models;

public class List 
{
    public int ListID { get; set; }
}

[PrimaryKey(nameof(ListID), nameof(ProductID))]
public class ListItem
{
    public int ListID { get; set; }
    public List List { get; set; } = null!;

    public int ProductID { get; set; }
    public Product Product { get; set; } = null!;
}