using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Models;

public class Gallery 
{
    public int GalleryID {get; set;}

    [Required]
    public string Location {get; set;} = null!;

    public string Description { get; set; } = null!;

    public Verifier[] GetVerifiers(DbSet<Verifier> VerifiersSet)
    {
        throw new NotImplementedException();
    }

    // Gallery Files CRUD
    public void AddFile(DbSet<GalleryFile> GalleryFileSet, File File)
    {
        throw new NotImplementedException();
    }
    
    public GalleryFile[] GetFiles(DbSet<GalleryFile> GalleryFileSet)
    {
        throw new NotImplementedException();
    }

    public void DeleteFile(DbSet<GalleryFile> GalleryFileSet, File File)
    {
        throw new NotImplementedException();
    }
}

[PrimaryKey(nameof(GalleryID), nameof(FileID))]
public class GalleryFile
{
    public int GalleryID { get; set; }
    public Gallery Gallery { get; set; } = null!;

    public File File { get; set; } = null!;
    public int FileID { get; set; }
    public bool Public { get; set; }
}