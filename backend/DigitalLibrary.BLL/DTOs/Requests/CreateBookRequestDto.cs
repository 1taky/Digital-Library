namespace DigitalLibrary.BLL.DTOs.Requests;

public class CreateBookRequestDto
{
    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string GenreName { get; set; } = string.Empty;

    public string Language { get; set; } = string.Empty;

    public int PublicationYear { get; set; }

    public List<BookFormatRequestDto> Formats { get; set; } = new();
}