namespace DigitalLibrary.BLL.DTOs.Responses;

public class BookFileResponseDto
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public string FileCategory { get; set; } = string.Empty;

    public DateTime UploadedAt { get; set; }
}