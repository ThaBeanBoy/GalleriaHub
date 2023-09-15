using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Review : IDateTime
{
    [Key]
    public int ReviewID { get; set; }

    [Required]
    [ForeignKey("User")]
    public required User Reviewer { get; set; }

    [Required]
    public required string ReviewHeader { get; set; }

    [Required]
    public required string ReviewContent { get; set; }

    public ICollection<Review>? Comments { get; set; }
    
    public DateTime CreatedOn { get; set; }
    
    public DateTime LastUpdate { get; set; }
}