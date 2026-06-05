using DigitalLibrary.DAL.Data;
using DigitalLibrary.DAL.Entities;
using DigitalLibrary.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace DigitalLibrary.IoC.Database;

public class DatabaseInitializer
{
    private readonly DigitalLibraryDbContext _context;

    public DatabaseInitializer(DigitalLibraryDbContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync()
    {
        await _context.Database.EnsureCreatedAsync();

        await SeedUsersAsync();
        await SeedGenresAsync();
        await SeedBooksAsync();
    }

    private async Task SeedUsersAsync()
    {
        bool adminExists = await _context.Users
            .AnyAsync(user => user.NormalizedEmail == "ADMIN@DIGITALLIBRARY.COM");

        if (!adminExists)
        {
            User admin = new User
            {
                FullName = "System Admin",
                Email = "admin@digitallibrary.com",
                NormalizedEmail = "ADMIN@DIGITALLIBRARY.COM",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin12345"),
                Role = Role.Admin,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Users.AddAsync(admin);
        }

        bool managerExists = await _context.Users
            .AnyAsync(user => user.NormalizedEmail == "MANAGER@DIGITALLIBRARY.COM");

        if (!managerExists)
        {
            User manager = new User
            {
                FullName = "Library Manager",
                Email = "manager@digitallibrary.com",
                NormalizedEmail = "MANAGER@DIGITALLIBRARY.COM",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Manager12345"),
                Role = Role.Manager,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Users.AddAsync(manager);
        }

        bool userExists = await _context.Users
            .AnyAsync(user => user.NormalizedEmail == "USER@DIGITALLIBRARY.COM");

        if (!userExists)
        {
            User user = new User
            {
                FullName = "Test User",
                Email = "user@digitallibrary.com",
                NormalizedEmail = "USER@DIGITALLIBRARY.COM",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("User12345"),
                Role = Role.User,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Users.AddAsync(user);
        }

        await _context.SaveChangesAsync();
    }

    private async Task SeedGenresAsync()
    {
        List<string> genreNames = new List<string>
        {
            "Fantasy",
            "Science Fiction",
            "Detective",
            "Self-development",
            "Programming",
            "History",
            "Romance",
            "Horror"
        };

        foreach (string genreName in genreNames)
        {
            string normalizedGenreName = genreName.Trim().ToUpper();

            bool genreExists = await _context.Genres
                .AnyAsync(genre => genre.Name.ToUpper() == normalizedGenreName);

            if (!genreExists)
            {
                Genre genre = new Genre
                {
                    Name = genreName
                };

                await _context.Genres.AddAsync(genre);
            }
        }

        await _context.SaveChangesAsync();
    }

    private async Task SeedBooksAsync()
    {
        Genre fantasy = await GetGenreByNameAsync("Fantasy");
        Genre selfDevelopment = await GetGenreByNameAsync("Self-development");
        Genre programming = await GetGenreByNameAsync("Programming");
        Genre detective = await GetGenreByNameAsync("Detective");
        Genre scienceFiction = await GetGenreByNameAsync("Science Fiction");
        Genre history = await GetGenreByNameAsync("History");

        await AddBookIfNotExistsAsync(
            title: "Atomic Habits",
            author: "James Clear",
            description: "A practical book about building good habits and breaking bad ones.",
            genre: selfDevelopment,
            language: "English",
            publicationYear: 2018,
            formats: new List<BookFormat>
            {
                new BookFormat
                {
                    FormatType = BookFormatType.Paper,
                    IsAvailable = true,
                    PagesCount = 320,
                    DurationMinutes = null
                },
                new BookFormat
                {
                    FormatType = BookFormatType.Electronic,
                    IsAvailable = true,
                    PagesCount = 320,
                    DurationMinutes = null
                },
                new BookFormat
                {
                    FormatType = BookFormatType.Audio,
                    IsAvailable = true,
                    PagesCount = null,
                    DurationMinutes = 480
                }
            });

        await AddBookIfNotExistsAsync(
            title: "Clean Code",
            author: "Robert C. Martin",
            description: "A book about writing clean, readable and maintainable code.",
            genre: programming,
            language: "English",
            publicationYear: 2008,
            formats: new List<BookFormat>
            {
                new BookFormat
                {
                    FormatType = BookFormatType.Paper,
                    IsAvailable = true,
                    PagesCount = 464,
                    DurationMinutes = null
                },
                new BookFormat
                {
                    FormatType = BookFormatType.Electronic,
                    IsAvailable = true,
                    PagesCount = 464,
                    DurationMinutes = null
                }
            });

        await AddBookIfNotExistsAsync(
            title: "The Pragmatic Programmer",
            author: "Andrew Hunt, David Thomas",
            description: "A well-known programming book about practical software development principles.",
            genre: programming,
            language: "English",
            publicationYear: 1999,
            formats: new List<BookFormat>
            {
                new BookFormat
                {
                    FormatType = BookFormatType.Electronic,
                    IsAvailable = true,
                    PagesCount = 352,
                    DurationMinutes = null
                }
            });

        await AddBookIfNotExistsAsync(
            title: "Harry Potter and the Philosopher's Stone",
            author: "J. K. Rowling",
            description: "A fantasy novel about a young wizard and his first year at Hogwarts.",
            genre: fantasy,
            language: "English",
            publicationYear: 1997,
            formats: new List<BookFormat>
            {
                new BookFormat
                {
                    FormatType = BookFormatType.Paper,
                    IsAvailable = true,
                    PagesCount = 320,
                    DurationMinutes = null
                },
                new BookFormat
                {
                    FormatType = BookFormatType.Audio,
                    IsAvailable = true,
                    PagesCount = null,
                    DurationMinutes = 510
                }
            });

        await AddBookIfNotExistsAsync(
            title: "Dune",
            author: "Frank Herbert",
            description: "A science fiction novel about politics, power and survival on the desert planet Arrakis.",
            genre: scienceFiction,
            language: "English",
            publicationYear: 1965,
            formats: new List<BookFormat>
            {
                new BookFormat
                {
                    FormatType = BookFormatType.Paper,
                    IsAvailable = true,
                    PagesCount = 688,
                    DurationMinutes = null
                },
                new BookFormat
                {
                    FormatType = BookFormatType.Electronic,
                    IsAvailable = true,
                    PagesCount = 688,
                    DurationMinutes = null
                },
                new BookFormat
                {
                    FormatType = BookFormatType.Audio,
                    IsAvailable = true,
                    PagesCount = null,
                    DurationMinutes = 1260
                }
            });

        await AddBookIfNotExistsAsync(
            title: "Sherlock Holmes: A Study in Scarlet",
            author: "Arthur Conan Doyle",
            description: "The first detective story featuring Sherlock Holmes and Dr. Watson.",
            genre: detective,
            language: "English",
            publicationYear: 1887,
            formats: new List<BookFormat>
            {
                new BookFormat
                {
                    FormatType = BookFormatType.Paper,
                    IsAvailable = true,
                    PagesCount = 188,
                    DurationMinutes = null
                },
                new BookFormat
                {
                    FormatType = BookFormatType.Electronic,
                    IsAvailable = true,
                    PagesCount = 188,
                    DurationMinutes = null
                }
            });

        await AddBookIfNotExistsAsync(
            title: "Sapiens: A Brief History of Humankind",
            author: "Yuval Noah Harari",
            description: "A popular science book about the history and development of humankind.",
            genre: history,
            language: "English",
            publicationYear: 2011,
            formats: new List<BookFormat>
            {
                new BookFormat
                {
                    FormatType = BookFormatType.Paper,
                    IsAvailable = true,
                    PagesCount = 498,
                    DurationMinutes = null
                },
                new BookFormat
                {
                    FormatType = BookFormatType.Audio,
                    IsAvailable = true,
                    PagesCount = null,
                    DurationMinutes = 930
                }
            });

        await _context.SaveChangesAsync();
    }

    private async Task AddBookIfNotExistsAsync(
        string title,
        string author,
        string description,
        Genre genre,
        string language,
        int publicationYear,
        List<BookFormat> formats)
    {
        string normalizedTitle = title.Trim().ToUpper();
        string normalizedAuthor = author.Trim().ToUpper();
        string normalizedLanguage = language.Trim().ToUpper();

        bool bookExists = await _context.Books
            .AnyAsync(book =>
                book.Title.ToUpper() == normalizedTitle &&
                book.Author.ToUpper() == normalizedAuthor &&
                book.Language.ToUpper() == normalizedLanguage &&
                book.PublicationYear == publicationYear);

        if (bookExists)
        {
            return;
        }

        Book book = new Book
        {
            Title = title,
            Author = author,
            Description = description,
            GenreId = genre.Id,
            Language = language,
            PublicationYear = publicationYear,
            CreatedAt = DateTime.UtcNow,
            Formats = formats
        };

        await _context.Books.AddAsync(book);
    }

    private async Task<Genre> GetGenreByNameAsync(string name)
    {
        string normalizedName = name.Trim().ToUpper();

        Genre? genre = await _context.Genres
            .FirstOrDefaultAsync(genre => genre.Name.ToUpper() == normalizedName);

        if (genre == null)
        {
            throw new InvalidOperationException($"Жанр '{name}' не знайдено.");
        }

        return genre;
    }
}