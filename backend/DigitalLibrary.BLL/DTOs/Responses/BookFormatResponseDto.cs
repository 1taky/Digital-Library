namespace DigitalLibrary.BLL.DTOs.Responses;

public class BookFormatResponseDto
{
    public int Id { get; set; }

    public string FormatType { get; set; } = string.Empty;

    public bool IsAvailable { get; set; }

    public int? PagesCount { get; set; }

    public int? DurationMinutes { get; set; }
}