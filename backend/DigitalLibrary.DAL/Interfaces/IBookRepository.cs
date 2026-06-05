using DigitalLibrary.DAL.Entities;
using DigitalLibrary.DAL.Enums;

namespace DigitalLibrary.DAL.Interfaces;

public interface IBookRepository : IGenericRepository<Book>
{
    Task<Book?> GetByIdDetailedAsync(int id);

    Task<List<Book>> GetAllDetailedAsync();

    Task<List<Book>> GetFilteredAsync(
        string? search,
        int? genreId,
        BookFormatType? formatType,
        string? sortBy,
        string? sortDirection);

    Task<bool> ExistsDuplicateAsync(
        string title,
        string author,
        string language,
        int publicationYear);
}