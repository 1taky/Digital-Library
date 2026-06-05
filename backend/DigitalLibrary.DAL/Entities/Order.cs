using DigitalLibrary.DAL.Enums;

namespace DigitalLibrary.DAL.Entities;

public class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public int BookId { get; set; }

    public Book Book { get; set; } = null!;

    public string PhoneNumber { get; set; } = string.Empty;

    public DateTime? BorrowedAt { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? ReturnedAt { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Requested;

    public int? ManagerId { get; set; }

    public User? Manager { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}