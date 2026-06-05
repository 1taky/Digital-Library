namespace DigitalLibrary.API.Models.Responses;

public class BookOrderStatusResponseModel
{
    public int BookId { get; set; }

    public bool HasActiveOrder { get; set; }

    public int? OrderId { get; set; }

    public string? Status { get; set; }
}