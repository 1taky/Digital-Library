namespace DigitalLibrary.BLL.DTOs.Requests;

public class BookFilterRequestDto
{
    public string? Search { get; set; }

    public string? GenreName { get; set; }

    public string? FormatType { get; set; }

    public int? Cursor { get; set; }

    public int PageSize { get; set; } = 10;
}