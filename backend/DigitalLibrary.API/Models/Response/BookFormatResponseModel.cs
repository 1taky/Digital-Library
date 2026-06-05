namespace DigitalLibrary.API.Models.Responses;

public class BookFormatResponseModel
{
    public int Id { get; set; }

    public string FormatType { get; set; } = string.Empty;

    public bool IsAvailable { get; set; }

    public int? PagesCount { get; set; }

    public int? DurationMinutes { get; set; }
}