using DigitalLibrary.DAL.Entities;
using DigitalLibrary.DAL.Enums;

namespace DigitalLibrary.DAL.Interfaces;

public interface IBookFileRepository : IGenericRepository<BookFile>
{
    Task<BookFile?> GetByBookIdAndCategoryAsync(
        int bookId,
        FileCategory fileCategory);

    Task<List<BookFile>> GetByBookIdAsync(int bookId);
}