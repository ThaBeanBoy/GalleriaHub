using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Order
{
    public int OrderID {get; set;}

    [Required]
    public DateTime OrderDate {get; set;}

    [Required, ForeignKey("User")]
    public required User Customer {get; set;}
}