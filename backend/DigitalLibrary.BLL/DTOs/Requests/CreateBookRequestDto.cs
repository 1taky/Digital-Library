namespace DigitalLibrary.BLL.DTOs.Requests;

public class CreateBookRequestDto
{
    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string BookType { get; set; } = string.Empty;

    public int GenreId { get; set; }

    public string Language { get; set; } = string.Empty;

    public int PublicationYear { get; set; }

    public int? PagesCount { get; set; }

    public int? DurationMinutes { get; set; }
}