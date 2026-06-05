using DigitalLibrary.DAL.Enums;

namespace DigitalLibrary.DAL.Entities;

public class BookFormat
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public Book Book { get; set; } = null!;

    public BookFormatType FormatType { get; set; }

    public bool IsAvailable { get; set; } = true;

    public int? PagesCount { get; set; }

    public int? DurationMinutes { get; set; }
}