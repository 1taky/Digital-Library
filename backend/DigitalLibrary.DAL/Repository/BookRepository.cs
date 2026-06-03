using DigitalLibrary.DAL.Data;
using DigitalLibrary.DAL.Entities;
using DigitalLibrary.DAL.Enums;
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

    public async Task<List<Book>> GetFilteredAsync(
        string? search,
        int? genreId,
        BookType? bookType,
        string? sortBy,
        string? sortDirection)
    {
        IQueryable<Book> query = DbSet
            .Include(book => book.Genre)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            string normalizedSearch = search.Trim().ToLower();

            query = query.Where(book =>
                book.Title.ToLower().Contains(normalizedSearch) ||
                book.Author.ToLower().Contains(normalizedSearch) ||
                book.Description.ToLower().Contains(normalizedSearch));
        }

        if (genreId.HasValue)
        {
            query = query.Where(book => book.GenreId == genreId.Value);
        }

        if (bookType.HasValue)
        {
            query = query.Where(book => book.BookType == bookType.Value);
        }

        bool descending = string.Equals(
            sortDirection,
            "desc",
            StringComparison.OrdinalIgnoreCase);

        query = sortBy?.Trim().ToLower() switch
        {
            "title" => descending
                ? query.OrderByDescending(book => book.Title)
                : query.OrderBy(book => book.Title),

            "author" => descending
                ? query.OrderByDescending(book => book.Author)
                : query.OrderBy(book => book.Author),

            "year" => descending
                ? query.OrderByDescending(book => book.PublicationYear)
                : query.OrderBy(book => book.PublicationYear),

            "type" => descending
                ? query.OrderByDescending(book => book.BookType)
                : query.OrderBy(book => book.BookType),

            "genre" => descending
                ? query.OrderByDescending(book => book.Genre.Name)
                : query.OrderBy(book => book.Genre.Name),

            _ => query.OrderBy(book => book.Title)
        };

        return await query.ToListAsync();
    }
}