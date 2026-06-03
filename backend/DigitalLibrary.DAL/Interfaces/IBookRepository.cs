using DigitalLibrary.DAL.Entities;
using DigitalLibrary.DAL.Enums;

namespace DigitalLibrary.DAL.Interfaces;

public interface IBookRepository : IGenericRepository<Book>
{
    Task<Book?> GetByIdWithGenreAsync(int id);

    Task<List<Book>> GetAllWithGenreAsync();

    Task<List<Book>> GetFilteredAsync(
        string? search,
        int? genreId,
        BookType? bookType,
        string? sortBy,
        string? sortDirection);
}