using System.ComponentModel.DataAnnotations;

namespace Models;


public class ProductImage
{
    public int ProductImageID { get; set; }

    public byte[] File { get; set; } = null!;
}