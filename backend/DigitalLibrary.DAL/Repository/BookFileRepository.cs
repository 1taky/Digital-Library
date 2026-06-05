using DigitalLibrary.DAL.Data;
using DigitalLibrary.DAL.Entities;
using DigitalLibrary.DAL.Enums;
using DigitalLibrary.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalLibrary.DAL.Repositories;

public class BookFileRepository : GenericRepository<BookFile>, IBookFileRepository
{
    public BookFileRepository(DigitalLibraryDbContext context)
        : base(context)
    {
    }

    public async Task<BookFile?> GetByBookIdAndCategoryAsync(
        int bookId,
        FileCategory fileCategory)
    {
        return await DbSet.FirstOrDefaultAsync(file =>
            file.BookId == bookId &&
            file.FileCategory == fileCategory);
    }

    public async Task<List<BookFile>> GetByBookIdAsync(int bookId)
    {
        return await DbSet
            .Where(file => file.BookId == bookId)
            .ToListAsync();
    }
}