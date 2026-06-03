namespace DigitalLibrary.BLL.DTOs.Requests;

public class BookFilterRequestDto
{
    public string? Search { get; set; }

    public int? GenreId { get; set; }

    public string? BookType { get; set; }

    public string? SortBy { get; set; }

    public string? SortDirection { get; set; }
}