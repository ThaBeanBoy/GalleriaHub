using Microsoft.EntityFrameworkCore;

namespace Models;

public interface IList<IListItem>
{
    void AddItem(IListItem NewItem);

    void RemoveItem(IListItem NewItem);

    public IListItem[] GetItems();
}

public class List : IDateTime, IList<ListItem>
{
    public int ListID { get; set; }

    public DateTime CreatedOn { get; set; }
    
    public DateTime LastUpdate { get; set; }

    public void AddItem(ListItem NewItem)
    {
        throw new NotImplementedException();
    }

    public ListItem[] GetItems()
    {
        throw new NotImplementedException();
    }

    public void RemoveItem(ListItem NewItem)
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