namespace DigitalLibrary.API.Models.Requests;

public class UpdateBookRequestModel
{
    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int GenreId { get; set; }

    public string Language { get; set; } = string.Empty;

    public int PublicationYear { get; set; }

    public List<BookFormatRequestModel> Formats { get; set; } = new();
}