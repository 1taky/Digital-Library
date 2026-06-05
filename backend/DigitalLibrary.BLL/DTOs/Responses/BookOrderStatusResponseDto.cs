namespace DigitalLibrary.BLL.DTOs.Responses;

public class BookOrderStatusResponseDto
{
    public int BookId { get; set; }

    public bool HasActiveOrder { get; set; }

    public int? OrderId { get; set; }

    public string? Status { get; set; }
}