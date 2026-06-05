using DigitalLibrary.BLL.DTOs.Requests;
using DigitalLibrary.BLL.DTOs.Responses;

namespace DigitalLibrary.BLL.Interfaces;

public interface IOrderService
{
    Task<OrderResponseDto> RequestAsync(
        CreateOrderRequestDto request,
        int userId);

    Task<List<OrderResponseDto>> GetMyOrdersAsync(int userId);

    Task<List<OrderResponseDto>> GetAllAsync();

    Task<List<OrderResponseDto>> GetOverdueAsync();

    Task<OrderResponseDto> GetByIdAsync(
        int orderId,
        int currentUserId,
        string currentUserRole);

    Task<BookOrderStatusResponseDto> GetActiveOrderByBookIdAsync(int bookId);
    Task<OrderResponseDto> ApproveAsync(int orderId, int managerId);

    Task<OrderResponseDto> BorrowAsync(int orderId, int managerId);

    Task<OrderResponseDto> ReturnAsync(int orderId, int managerId);

    Task<OrderResponseDto> RejectAsync(int orderId, int managerId);
}