using DigitalLibrary.DAL.Data;
using DigitalLibrary.DAL.Entities;
using DigitalLibrary.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalLibrary.DAL.Repositories;

public class BookRepository : GenericRepository<Book>, IBookRepository
{
    public BookRepository(DigitalLibraryDbContext context)
        : base(context)
    {
    }

    public async Task<Book?> GetByIdWithGenreAsync(int id)
    {
        return await DbSet
            .Include(book => book.Genre)
            .FirstOrDefaultAsync(book => book.Id == id);
    }

    public async Task<List<Book>> GetAllWithGenreAsync()
    {
        return await DbSet
            .Include(book => book.Genre)
            .OrderBy(book => book.Title)
            .ToListAsync();
    }
}