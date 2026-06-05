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

    public async Task<Book?> GetByIdDetailedAsync(int id)
    {
        return await DbSet
            .Include(book => book.Genre)
            .Include(book => book.Formats)
            .Include(book => book.Files)
            .FirstOrDefaultAsync(book => book.Id == id);
    }

    public async Task<List<Book>> GetAllDetailedAsync()
    {
        return await DbSet
            .Include(book => book.Genre)
            .Include(book => book.Formats)
            .Include(book => book.Files)
            .OrderBy(book => book.Title)
            .ToListAsync();
    }

    public async Task<List<Book>> GetFilteredAsync(
        string? search,
        int? genreId,
        BookFormatType? formatType,
        string? sortBy,
        string? sortDirection)
    {
        IQueryable<Book> query = DbSet
            .Include(book => book.Genre)
            .Include(book => book.Formats)
            .Include(book => book.Files)
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

        if (formatType.HasValue)
        {
            query = query.Where(book =>
                book.Formats.Any(format => format.FormatType == formatType.Value));
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

            "genre" => descending
                ? query.OrderByDescending(book => book.Genre.Name)
                : query.OrderBy(book => book.Genre.Name),

            _ => query.OrderBy(book => book.Title)
        };

        return await query.ToListAsync();
    }

    public async Task<bool> ExistsDuplicateAsync(
        string title,
        string author,
        string language,
        int publicationYear)
    {
        string normalizedTitle = title.Trim().ToLower();
        string normalizedAuthor = author.Trim().ToLower();
        string normalizedLanguage = language.Trim().ToLower();

        return await DbSet.AnyAsync(book =>
            book.Title.ToLower() == normalizedTitle &&
            book.Author.ToLower() == normalizedAuthor &&
            book.Language.ToLower() == normalizedLanguage &&
            book.PublicationYear == publicationYear);
    }
}