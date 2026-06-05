using DigitalLibrary.DAL.Data;
using DigitalLibrary.DAL.Entities;
using DigitalLibrary.DAL.Enums;
using DigitalLibrary.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalLibrary.DAL.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(DigitalLibraryDbContext context)
        : base(context)
    {
    }

    public async Task<Order?> GetByIdDetailedAsync(int id)
    {
        return await DbSet
            .Include(order => order.User)
            .Include(order => order.Manager)
            .Include(order => order.Book)
                .ThenInclude(book => book.Genre)
            .Include(order => order.Book)
                .ThenInclude(book => book.Formats)
            .FirstOrDefaultAsync(order => order.Id == id);
    }

    public async Task<List<Order>> GetAllDetailedAsync()
    {
        return await DbSet
            .Include(order => order.User)
            .Include(order => order.Manager)
            .Include(order => order.Book)
                .ThenInclude(book => book.Genre)
            .Include(order => order.Book)
                .ThenInclude(book => book.Formats)
            .OrderByDescending(order => order.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Order>> GetByUserIdDetailedAsync(int userId)
    {
        return await DbSet
            .Include(order => order.User)
            .Include(order => order.Manager)
            .Include(order => order.Book)
                .ThenInclude(book => book.Genre)
            .Include(order => order.Book)
                .ThenInclude(book => book.Formats)
            .Where(order => order.UserId == userId)
            .OrderByDescending(order => order.CreatedAt)
            .ToListAsync();
    }

    public async Task<Order?> GetActiveByBookIdDetailedAsync(int bookId)
    {
        return await DbSet
            .Include(order => order.User)
            .Include(order => order.Manager)
            .Include(order => order.Book)
                .ThenInclude(book => book.Genre)
            .Include(order => order.Book)
                .ThenInclude(book => book.Formats)
            .Where(order =>
                order.BookId == bookId &&
                (
                    order.Status == OrderStatus.Requested ||
                    order.Status == OrderStatus.Approved ||
                    order.Status == OrderStatus.Borrowed ||
                    order.Status == OrderStatus.Overdue
                ))
            .OrderByDescending(order => order.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> HasActiveOrderForBookAsync(int bookId)
    {
        return await DbSet.AnyAsync(order =>
            order.BookId == bookId &&
            (
                order.Status == OrderStatus.Requested ||
                order.Status == OrderStatus.Approved ||
                order.Status == OrderStatus.Borrowed ||
                order.Status == OrderStatus.Overdue
            ));
    }

    public async Task<List<Order>> GetOverdueDetailedAsync()
    {
        return await DbSet
            .Include(order => order.User)
            .Include(order => order.Manager)
            .Include(order => order.Book)
                .ThenInclude(book => book.Genre)
            .Include(order => order.Book)
                .ThenInclude(book => book.Formats)
            .Where(order => order.Status == OrderStatus.Overdue)
            .OrderBy(order => order.DueDate)
            .ToListAsync();
    }
}