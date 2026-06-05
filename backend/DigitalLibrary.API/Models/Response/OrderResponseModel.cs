namespace DigitalLibrary.API.Models.Responses;

public class OrderResponseModel
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public string BookTitle { get; set; } = string.Empty;

    public int UserId { get; set; }

    public string UserFullName { get; set; } = string.Empty;

    public string UserEmail { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public DateTime? BorrowedAt { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? ReturnedAt { get; set; }

    public string Status { get; set; } = string.Empty;

    public int? ManagerId { get; set; }

    public string? ManagerFullName { get; set; }

    public DateTime CreatedAt { get; set; }
}