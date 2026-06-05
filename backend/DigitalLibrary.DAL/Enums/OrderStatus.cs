namespace DigitalLibrary.DAL.Enums;

public enum OrderStatus
{
    Requested = 0,
    Approved = 1,
    Borrowed = 2,
    Returned = 3,
    Overdue = 4,
    Rejected = 5
}