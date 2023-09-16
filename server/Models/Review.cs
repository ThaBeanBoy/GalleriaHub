using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Review : IDateTime
{
    [Key]
    public int ReviewID { get; set; }

    public User User { get; set; } = null!;
    public int UserID { get; set; } 

    public string ReviewHeader { get; set; } = null!;

    public string ReviewContent { get; set; } = null!;
    
    public DateTime CreatedOn { get; set; }
    
    public DateTime LastUpdate { get; set; }
}