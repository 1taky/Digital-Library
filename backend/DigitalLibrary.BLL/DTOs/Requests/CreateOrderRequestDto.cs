namespace DigitalLibrary.BLL.DTOs.Requests;

public class CreateOrderRequestDto
{
    public int BookId { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;
}