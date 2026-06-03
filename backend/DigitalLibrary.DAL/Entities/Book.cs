using DigitalLibrary.DAL.Enums;

namespace DigitalLibrary.DAL.Entities;

public class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public BookType BookType { get; set; }

    public int GenreId { get; set; }

    public Genre Genre { get; set; } = null!;

    public string Language { get; set; } = string.Empty;

    public int PublicationYear { get; set; }

    public int? PagesCount { get; set; }

    public int? DurationMinutes { get; set; }

    public bool IsAvailable { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}