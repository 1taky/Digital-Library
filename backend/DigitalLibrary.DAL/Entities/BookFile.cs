using DigitalLibrary.DAL.Enums;

namespace DigitalLibrary.DAL.Entities;

public class BookFile
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public Book Book { get; set; } = null!;

    public string FileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public FileCategory FileCategory { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}