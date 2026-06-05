namespace DigitalLibrary.API.Models.Requests;

public class BookFormatRequestModel
{
    public string FormatType { get; set; } = string.Empty;

    public int? PagesCount { get; set; }

    public int? DurationMinutes { get; set; }
}