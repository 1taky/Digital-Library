using DigitalLibrary.DAL.Entities;

namespace DigitalLibrary.DAL.Interfaces;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<Order?> GetByIdDetailedAsync(int id);

    Task<List<Order>> GetAllDetailedAsync();

    Task<List<Order>> GetByUserIdDetailedAsync(int userId);

    Task<List<Order>> GetOverdueDetailedAsync();

    Task<bool> HasActiveOrderForBookAsync(int bookId);
}