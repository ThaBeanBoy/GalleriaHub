using System.ComponentModel.DataAnnotations;

namespace Models;

public class Gallery 
{
    public int GalleryID {get; set;}

    [Required]
    public string Location {get; set;} = null!;

    public File CoverImage { get; set; } = null!;
    public int FileID { get; set; } 

    public string Description { get; set; } = null!;
}