using Microsoft.EntityFrameworkCore;

namespace Models;

public interface IList<IListItem> where IListItem : class
{
    // Create
    void AddItem(DbSet<IListItem> ListItemSet, IListItem NewItem);

    // Retrieve
    public IListItem[] GetItems(DbSet<IListItem> ListItemSet);

    // Delete
    void RemoveItem(DbSet<IListItem> ListItemSet, IListItem NewItem);
}

public class List : IDateTime, IList<ListItem>
{
    public int ListID { get; set; }

    public string Name {get; set; } = null!;
    public int UserID {get; set;}
    public User User { get; set; } = null!;
    public DateTime CreatedOn { get; set; }

    public string? Description { get; set; }
    
    public DateTime LastUpdate { get; set; }

    public void AddItem(DbSet<ListItem> ListItemSet, ListItem NewItem)
    {
        throw new NotImplementedException();
    }

    public ListItem[] GetItems(DbSet<ListItem> ListItemSet)
    {
        throw new NotImplementedException();
    }

    public void RemoveItem(DbSet<ListItem> ListItemSet, ListItem NewItem)
    {
        throw new NotImplementedException();
    }
}

[PrimaryKey(nameof(ListID), nameof(ProductID))]
public class ListItem
{
    public int ListID { get; set; }
    public List List { get; set; } = null!;

    public int ProductID { get; set; }
    public Product Product { get; set; } = null!;
}