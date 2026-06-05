using DigitalLibrary.DAL.Entities;
using DigitalLibrary.DAL.Enums;

namespace DigitalLibrary.DAL.Interfaces;

public interface IBookRepository : IGenericRepository<Book>
{
    Task<Book?> GetByIdDetailedAsync(int id);

    Task<List<Book>> GetAllDetailedAsync();

    Task<(List<Book> Items, bool HasMore)> GetFilteredByCursorAsync(
        string? search,
        string? genreName,
        BookFormatType? formatType,
        int? cursor,
        int pageSize);

    Task<bool> ExistsDuplicateAsync(
        string title,
        string author,
        string language,
        int publicationYear);
}