namespace DigitalLibrary.API.Models.Responses;

public class BookResponseModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int GenreId { get; set; }

    public string GenreName { get; set; } = string.Empty;

    public string Language { get; set; } = string.Empty;

    public int PublicationYear { get; set; }

    public List<BookFormatResponseModel> Formats { get; set; } = new();

    public string? CoverUrl { get; set; }

    public string? DownloadUrl { get; set; }

    public string? ListenUrl { get; set; }

    public DateTime CreatedAt { get; set; }
}