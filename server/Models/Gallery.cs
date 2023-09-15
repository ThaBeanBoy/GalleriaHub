using System.ComponentModel.DataAnnotations;

namespace Models;

public class Gallery 
{
    public int GalleryID {get; set;}

    [Required]
    public string Location {get; set;} = null!;

    public byte[]? CoverImage { get; set; } 
}