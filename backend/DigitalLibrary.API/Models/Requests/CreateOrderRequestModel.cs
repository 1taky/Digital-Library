namespace DigitalLibrary.API.Models.Requests;

public class CreateOrderRequestModel
{
    public int BookId { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;
}