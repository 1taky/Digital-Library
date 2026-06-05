namespace DigitalLibrary.API.Models.Requests;

public class BookFilterRequestModel
{
    public string? Search { get; set; }

    public int? GenreId { get; set; }

    public string? FormatType { get; set; }

    public string? SortBy { get; set; }

    public string? SortDirection { get; set; }
}