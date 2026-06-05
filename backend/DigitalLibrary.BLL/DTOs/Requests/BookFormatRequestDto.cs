namespace DigitalLibrary.BLL.DTOs.Requests;

public class BookFormatRequestDto
{
    public string FormatType { get; set; } = string.Empty;

    public int? PagesCount { get; set; }

    public int? DurationMinutes { get; set; }
}