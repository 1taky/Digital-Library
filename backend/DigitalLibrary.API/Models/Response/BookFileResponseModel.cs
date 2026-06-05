namespace DigitalLibrary.API.Models.Responses;

public class BookFileResponseModel
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public string FileCategory { get; set; } = string.Empty;

    public DateTime UploadedAt { get; set; }
}