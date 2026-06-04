namespace DigitalLibrary.BLL.DTOs.Responses;

public class BookResponseDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string BookType { get; set; } = string.Empty;

    public string GenreName { get; set; } = string.Empty;

    public string Language { get; set; } = string.Empty;

    public int PublicationYear { get; set; }

    public int? PagesCount { get; set; }

    public int? DurationMinutes { get; set; }

    public bool IsAvailable { get; set; }

    public DateTime CreatedAt { get; set; }
}