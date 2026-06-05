namespace DigitalLibrary.DAL.Entities;

public class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int GenreId { get; set; }

    public Genre Genre { get; set; } = null!;

    public string Language { get; set; } = string.Empty;

    public int PublicationYear { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<BookFormat> Formats { get; set; } = new();

    public List<BookFile> Files { get; set; } = new();
}