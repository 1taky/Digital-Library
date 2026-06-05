namespace DigitalLibrary.BLL.DTOs.Responses;

public class BookResponseDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int GenreId { get; set; }

    public string GenreName { get; set; } = string.Empty;

    public string Language { get; set; } = string.Empty;

    public int PublicationYear { get; set; }

    public List<BookFormatResponseDto> Formats { get; set; } = new();

    public bool HasCover { get; set; }

    public bool HasDownloadFile { get; set; }

    public bool HasAudioFile { get; set; }

    public DateTime CreatedAt { get; set; }
}