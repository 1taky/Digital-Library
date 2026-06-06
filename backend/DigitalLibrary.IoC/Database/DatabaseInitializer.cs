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
        Genre romance = await GetGenreByNameAsync("Romance");
        Genre horror = await GetGenreByNameAsync("Horror");

        await AddBookIfNotExistsAsync(
            title: "Harry Potter and the Philosopher's Stone",
            author: "J. K. Rowling",
            description: "A fantasy novel about a young wizard and his first year at Hogwarts.",
            genre: fantasy,
            language: "English",
            publicationYear: 1997,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 320 },
                new BookFormat { FormatType = BookFormatType.Audio, IsAvailable = true, DurationMinutes = 510 }
            });

        await AddBookIfNotExistsAsync(
            title: "The Lord of the Rings: The Fellowship of the Ring",
            author: "J. R. R. Tolkien",
            description: "An epic high-fantasy novel following the quest to destroy the One Ring.",
            genre: fantasy,
            language: "English",
            publicationYear: 1954,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 423 },
                new BookFormat { FormatType = BookFormatType.Electronic, IsAvailable = true, PagesCount = 423 }
            });

        await AddBookIfNotExistsAsync(
            title: "A Game of Thrones",
            author: "George R. R. Martin",
            description: "The first book in A Song of Ice and Fire series, a tale of lords, ladies, soldiers, and sorcerers.",
            genre: fantasy,
            language: "English",
            publicationYear: 1996,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 694 },
                new BookFormat { FormatType = BookFormatType.Electronic, IsAvailable = true, PagesCount = 694 },
                new BookFormat { FormatType = BookFormatType.Audio, IsAvailable = true, DurationMinutes = 2020 }
            });

        await AddBookIfNotExistsAsync(
            title: "Mistborn: The Final Empire",
            author: "Brandon Sanderson",
            description: "A world where ash falls from the sky and certain people have magical abilities based on consuming metals.",
            genre: fantasy,
            language: "English",
            publicationYear: 2006,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 541 },
                new BookFormat { FormatType = BookFormatType.Electronic, IsAvailable = true, PagesCount = 541 }
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
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 688 },
                new BookFormat { FormatType = BookFormatType.Electronic, IsAvailable = true, PagesCount = 688 },
                new BookFormat { FormatType = BookFormatType.Audio, IsAvailable = true, DurationMinutes = 1260 }
            });

        await AddBookIfNotExistsAsync(
            title: "The Martian",
            author: "Andy Weir",
            description: "An astronaut is stranded on Mars and must use his scientific knowledge to survive.",
            genre: scienceFiction,
            language: "English",
            publicationYear: 2011,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Electronic, IsAvailable = true, PagesCount = 369 },
                new BookFormat { FormatType = BookFormatType.Audio, IsAvailable = true, DurationMinutes = 650 }
            });

        await AddBookIfNotExistsAsync(
            title: "Foundation",
            author: "Isaac Asimov",
            description: "A mathematician develops 'psychohistory' to predict the future and save humanity from a dark age.",
            genre: scienceFiction,
            language: "English",
            publicationYear: 1951,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 255 },
                new BookFormat { FormatType = BookFormatType.Electronic, IsAvailable = true, PagesCount = 255 }
            });

        await AddBookIfNotExistsAsync(
            title: "1984",
            author: "George Orwell",
            description: "A dystopian social science fiction novel and cautionary tale about the dangers of totalitarianism.",
            genre: scienceFiction,
            language: "English",
            publicationYear: 1949,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 328 },
                new BookFormat { FormatType = BookFormatType.Audio, IsAvailable = true, DurationMinutes = 680 }
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
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 188 },
                new BookFormat { FormatType = BookFormatType.Electronic, IsAvailable = true, PagesCount = 188 }
            });

        await AddBookIfNotExistsAsync(
            title: "And Then There Were None",
            author: "Agatha Christie",
            description: "Ten strangers are invited to an isolated island and killed off one by one.",
            genre: detective,
            language: "English",
            publicationYear: 1939,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 272 },
                new BookFormat { FormatType = BookFormatType.Electronic, IsAvailable = true, PagesCount = 272 },
                new BookFormat { FormatType = BookFormatType.Audio, IsAvailable = true, DurationMinutes = 360 }
            });

        await AddBookIfNotExistsAsync(
            title: "The Girl with the Dragon Tattoo",
            author: "Stieg Larsson",
            description: "A journalist and a hacker investigate the disappearance of a wealthy family's niece.",
            genre: detective,
            language: "English",
            publicationYear: 2005,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 672 },
                new BookFormat { FormatType = BookFormatType.Electronic, IsAvailable = true, PagesCount = 672 }
            });

        await AddBookIfNotExistsAsync(
            title: "Gone Girl",
            author: "Gillian Flynn",
            description: "A thriller revolving around a husband whose wife disappears on their wedding anniversary.",
            genre: detective,
            language: "English",
            publicationYear: 2012,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 432 },
                new BookFormat { FormatType = BookFormatType.Audio, IsAvailable = true, DurationMinutes = 1150 }
            });

        await AddBookIfNotExistsAsync(
            title: "Atomic Habits",
            author: "James Clear",
            description: "A practical book about building good habits and breaking bad ones.",
            genre: selfDevelopment,
            language: "English",
            publicationYear: 2018,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 320 },
                new BookFormat { FormatType = BookFormatType.Electronic, IsAvailable = true, PagesCount = 320 },
                new BookFormat { FormatType = BookFormatType.Audio, IsAvailable = true, DurationMinutes = 480 }
            });

        await AddBookIfNotExistsAsync(
            title: "Deep Work",
            author: "Cal Newport",
            description: "Rules for focused success in a distracted world.",
            genre: selfDevelopment,
            language: "English",
            publicationYear: 2016,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 304 },
                new BookFormat { FormatType = BookFormatType.Electronic, IsAvailable = true, PagesCount = 304 }
            });

        await AddBookIfNotExistsAsync(
            title: "Thinking, Fast and Slow",
            author: "Daniel Kahneman",
            description: "An exploration of the two systems that drive the way we think.",
            genre: selfDevelopment,
            language: "English",
            publicationYear: 2011,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 499 },
                new BookFormat { FormatType = BookFormatType.Audio, IsAvailable = true, DurationMinutes = 1200 }
            });

        await AddBookIfNotExistsAsync(
            title: "The 7 Habits of Highly Effective People",
            author: "Stephen R. Covey",
            description: "A principle-centered approach for solving personal and professional problems.",
            genre: selfDevelopment,
            language: "English",
            publicationYear: 1989,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 381 },
                new BookFormat { FormatType = BookFormatType.Electronic, IsAvailable = true, PagesCount = 381 }
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
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 464 },
                new BookFormat { FormatType = BookFormatType.Electronic, IsAvailable = true, PagesCount = 464 }
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
                new BookFormat { FormatType = BookFormatType.Electronic, IsAvailable = true, PagesCount = 352 }
            });

        await AddBookIfNotExistsAsync(
            title: "Design Patterns: Elements of Reusable Object-Oriented Software",
            author: "Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides",
            description: "The classic 'Gang of Four' book that catalogs 23 design patterns.",
            genre: programming,
            language: "English",
            publicationYear: 1994,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 395 },
                new BookFormat { FormatType = BookFormatType.Electronic, IsAvailable = true, PagesCount = 395 }
            });

        await AddBookIfNotExistsAsync(
            title: "Refactoring: Improving the Design of Existing Code",
            author: "Martin Fowler",
            description: "A guide to improving the structure of code without changing its external behavior.",
            genre: programming,
            language: "English",
            publicationYear: 1999,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 448 },
                new BookFormat { FormatType = BookFormatType.Electronic, IsAvailable = true, PagesCount = 448 }
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
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 498 },
                new BookFormat { FormatType = BookFormatType.Audio, IsAvailable = true, DurationMinutes = 930 }
            });

        await AddBookIfNotExistsAsync(
            title: "Guns, Germs, and Steel",
            author: "Jared Diamond",
            description: "A transdisciplinary non-fiction book explaining geographic and environmental advantages in history.",
            genre: history,
            language: "English",
            publicationYear: 1997,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 480 },
                new BookFormat { FormatType = BookFormatType.Electronic, IsAvailable = true, PagesCount = 480 }
            });

        await AddBookIfNotExistsAsync(
            title: "SPQR: A History of Ancient Rome",
            author: "Mary Beard",
            description: "A sweeping revisionist history of the Roman Empire.",
            genre: history,
            language: "English",
            publicationYear: 2015,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 608 },
                new BookFormat { FormatType = BookFormatType.Audio, IsAvailable = true, DurationMinutes = 1020 }
            });

        await AddBookIfNotExistsAsync(
            title: "The Diary of a Young Girl",
            author: "Anne Frank",
            description: "The writings from the Dutch-language diary kept by Anne Frank while she was in hiding for two years with her family during the Nazi occupation.",
            genre: history,
            language: "English",
            publicationYear: 1947,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 283 },
                new BookFormat { FormatType = BookFormatType.Electronic, IsAvailable = true, PagesCount = 283 }
            });

        await AddBookIfNotExistsAsync(
            title: "Pride and Prejudice",
            author: "Jane Austen",
            description: "A romantic novel of manners following the character development of Elizabeth Bennet.",
            genre: romance,
            language: "English",
            publicationYear: 1813,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 432 },
                new BookFormat { FormatType = BookFormatType.Electronic, IsAvailable = true, PagesCount = 432 },
                new BookFormat { FormatType = BookFormatType.Audio, IsAvailable = true, DurationMinutes = 680 }
            });

        await AddBookIfNotExistsAsync(
            title: "The Notebook",
            author: "Nicholas Sparks",
            description: "A romantic novel based on a true story of a couple who fall in love during the summer of 1932.",
            genre: romance,
            language: "English",
            publicationYear: 1996,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 214 },
                new BookFormat { FormatType = BookFormatType.Electronic, IsAvailable = true, PagesCount = 214 }
            });

        await AddBookIfNotExistsAsync(
            title: "Outlander",
            author: "Diana Gabaldon",
            description: "A historical romance novel following a 20th-century nurse who time-travels to 18th-century Scotland.",
            genre: romance,
            language: "English",
            publicationYear: 1991,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 850 },
                new BookFormat { FormatType = BookFormatType.Audio, IsAvailable = true, DurationMinutes = 1950 }
            });

        await AddBookIfNotExistsAsync(
            title: "Me Before You",
            author: "Jojo Moyes",
            description: "A romantic novel about a young woman who becomes a caregiver for a paralyzed man.",
            genre: romance,
            language: "English",
            publicationYear: 2012,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Electronic, IsAvailable = true, PagesCount = 480 }
            });

        await AddBookIfNotExistsAsync(
            title: "The Shining",
            author: "Stephen King",
            description: "A family heads to an isolated hotel for the winter where a sinister presence influences the father into violence.",
            genre: horror,
            language: "English",
            publicationYear: 1977,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 447 },
                new BookFormat { FormatType = BookFormatType.Electronic, IsAvailable = true, PagesCount = 447 },
                new BookFormat { FormatType = BookFormatType.Audio, IsAvailable = true, DurationMinutes = 960 }
            });

        await AddBookIfNotExistsAsync(
            title: "Dracula",
            author: "Bram Stoker",
            description: "A gothic horror novel that tells the story of a vampire's attempt to move from Transylvania to England.",
            genre: horror,
            language: "English",
            publicationYear: 1897,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 418 },
                new BookFormat { FormatType = BookFormatType.Electronic, IsAvailable = true, PagesCount = 418 }
            });

        await AddBookIfNotExistsAsync(
            title: "It",
            author: "Stephen King",
            description: "Seven children are terrorized by an eponymous being, which exploits the fears of its victims.",
            genre: horror,
            language: "English",
            publicationYear: 1986,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Paper, IsAvailable = true, PagesCount = 1138 },
                new BookFormat { FormatType = BookFormatType.Audio, IsAvailable = true, DurationMinutes = 2690 }
            });

        await AddBookIfNotExistsAsync(
            title: "The Call of Cthulhu",
            author: "H. P. Lovecraft",
            description: "A short story featuring the first appearance of the extraterrestrial entity Cthulhu.",
            genre: horror,
            language: "English",
            publicationYear: 1928,
            formats: new List<BookFormat>
            {
                new BookFormat { FormatType = BookFormatType.Electronic, IsAvailable = true, PagesCount = 43 },
                new BookFormat { FormatType = BookFormatType.Audio, IsAvailable = true, DurationMinutes = 120 }
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