using AutoMapper;
using DigitalLibrary.BLL.DTOs.Requests;
using DigitalLibrary.BLL.DTOs.Responses;
using DigitalLibrary.BLL.Exceptions;
using DigitalLibrary.BLL.Interfaces;
using DigitalLibrary.DAL.Entities;
using DigitalLibrary.DAL.Enums;
using DigitalLibrary.DAL.Interfaces;

namespace DigitalLibrary.BLL.Services;

public class BookService : IBookService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BookService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<BookResponseDto>> GetAllAsync()
    {
        List<Book> books = await _unitOfWork.Books.GetAllDetailedAsync();

        return _mapper.Map<List<BookResponseDto>>(books);
    }

    public async Task<CursorPagedResultDto<BookResponseDto>> GetFilteredAsync(
    BookFilterRequestDto request)
{
    BookFormatType? parsedFormatType = null;

    if (!string.IsNullOrWhiteSpace(request.FormatType))
    {
        bool isValidFormatType = Enum.TryParse(
            request.FormatType,
            true,
            out BookFormatType formatType);

        if (!isValidFormatType)
        {
            throw new BadRequestException("Неправильний формат книги.");
        }

        parsedFormatType = formatType;
    }

    int pageSize = request.PageSize < 1
        ? 10
        : request.PageSize;

    if (pageSize > 50)
    {
        pageSize = 50;
    }

    (List<Book> books, bool hasMore) =
        await _unitOfWork.Books.GetFilteredByCursorAsync(
            request.Search,
            request.GenreName,
            parsedFormatType,
            request.Cursor,
            pageSize);

    List<BookResponseDto> mappedBooks =
        _mapper.Map<List<BookResponseDto>>(books);

    int? nextCursor = mappedBooks.Count == 0
        ? null
        : mappedBooks[^1].Id;

    return new CursorPagedResultDto<BookResponseDto>
    {
        Items = mappedBooks,
        NextCursor = hasMore ? nextCursor : null,
        HasMore = hasMore
    };
}

    public async Task<BookResponseDto> GetByIdAsync(int id)
    {
        Book? book = await _unitOfWork.Books.GetByIdDetailedAsync(id);

        if (book == null)
        {
            throw new NotFoundException("Книгу не знайдено.");
        }

        return _mapper.Map<BookResponseDto>(book);
    }

    public async Task<BookResponseDto> CreateAsync(CreateBookRequestDto request)
    {
        ValidateBookRequest(
            request.Title,
            request.Author,
            request.Description,
            request.GenreName,
            request.Language,
            request.PublicationYear,
            request.Formats);

        Genre? genre = await _unitOfWork.Genres.GetByNameAsync(request.GenreName);

        if (genre == null)
        {
            throw new NotFoundException("Жанр не знайдено.");
        }

        bool duplicateExists = await _unitOfWork.Books.ExistsDuplicateAsync(
            request.Title,
            request.Author,
            request.Language,
            request.PublicationYear);

        if (duplicateExists)
        {
            throw new BadRequestException("Така книга вже існує.");
        }

        Book book = _mapper.Map<Book>(request);

        book.GenreId = genre.Id;
        book.CreatedAt = DateTime.UtcNow;

        await _unitOfWork.Books.AddAsync(book);
        await _unitOfWork.SaveChangesAsync();

        Book createdBook = await _unitOfWork.Books.GetByIdDetailedAsync(book.Id)
            ?? throw new NotFoundException("Книгу не знайдено після створення.");

        return _mapper.Map<BookResponseDto>(createdBook);
    }

    public async Task<BookResponseDto> UpdateAsync(int id, UpdateBookRequestDto request)
    {
        ValidateBookRequest(
            request.Title,
            request.Author,
            request.Description,
            request.GenreName,
            request.Language,
            request.PublicationYear,
            request.Formats);

        Book? book = await _unitOfWork.Books.GetByIdDetailedAsync(id);

        if (book == null)
        {
            throw new NotFoundException("Книгу не знайдено.");
        }

        Genre? genre = await _unitOfWork.Genres.GetByNameAsync(request.GenreName);

        if (genre == null)
        {
            throw new NotFoundException("Жанр не знайдено.");
        }

        bool duplicateExists = await _unitOfWork.Books.ExistsDuplicateAsync(
            request.Title,
            request.Author,
            request.Language,
            request.PublicationYear);

        if (duplicateExists && !IsSameBook(book, request))
        {
            throw new BadRequestException("Інша книга з такими даними вже існує.");
        }

        book.Title = request.Title.Trim();
        book.Author = request.Author.Trim();
        book.Description = request.Description.Trim();
        book.GenreId = genre.Id;
        book.Language = request.Language.Trim();
        book.PublicationYear = request.PublicationYear;

        UpdateFormats(book, request.Formats);

        _unitOfWork.Books.Update(book);
        await _unitOfWork.SaveChangesAsync();

        Book updatedBook = await _unitOfWork.Books.GetByIdDetailedAsync(book.Id)
            ?? throw new NotFoundException("Книгу не знайдено після оновлення.");

        return _mapper.Map<BookResponseDto>(updatedBook);
    }

    public async Task DeleteAsync(int id)
    {
        Book? book = await _unitOfWork.Books.GetByIdAsync(id);

        if (book == null)
        {
            throw new NotFoundException("Книгу не знайдено.");
        }

        _unitOfWork.Books.Delete(book);
        await _unitOfWork.SaveChangesAsync();
    }

    private static bool IsSameBook(
    Book book,
    UpdateBookRequestDto request)
    {
        return string.Equals(book.Title.Trim(), request.Title.Trim(), StringComparison.OrdinalIgnoreCase) &&
               string.Equals(book.Author.Trim(), request.Author.Trim(), StringComparison.OrdinalIgnoreCase) &&
               string.Equals(book.Language.Trim(), request.Language.Trim(), StringComparison.OrdinalIgnoreCase) &&
               book.PublicationYear == request.PublicationYear;
    }

    private static void UpdateFormats(
        Book book,
        List<BookFormatRequestDto> requestedFormats)
    {
        book.Formats.Clear();

        foreach (BookFormatRequestDto formatRequest in requestedFormats)
        {
            BookFormatType formatType = Enum.Parse<BookFormatType>(
                formatRequest.FormatType,
                true);

            book.Formats.Add(new BookFormat
            {
                BookId = book.Id,
                FormatType = formatType,
                IsAvailable = true,
                PagesCount = formatRequest.PagesCount,
                DurationMinutes = formatRequest.DurationMinutes
            });
        }
    }

    private static void ValidateBookRequest(
    string title,
    string author,
    string description,
    string genreName,
    string language,
    int publicationYear,
    List<BookFormatRequestDto> formats)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new BadRequestException("Назва книги є обов'язковою.");
        }

        if (string.IsNullOrWhiteSpace(author))
        {
            throw new BadRequestException("Автор книги є обов'язковим.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new BadRequestException("Опис книги є обов'язковим.");
        }

        if (string.IsNullOrWhiteSpace(genreName))
        {
            throw new BadRequestException("Жанр книги є обов'язковим.");
        }

        if (string.IsNullOrWhiteSpace(language))
        {
            throw new BadRequestException("Мова книги є обов'язковою.");
        }

        if (publicationYear < 1000 || publicationYear > DateTime.UtcNow.Year)
        {
            throw new BadRequestException("Некоректний рік публікації.");
        }

        ValidateFormats(formats);
    }

    private static void ValidateFormats(List<BookFormatRequestDto> formats)
    {
        if (formats.Count == 0)
        {
            throw new BadRequestException("Потрібно вказати хоча б один формат книги.");
        }

        HashSet<BookFormatType> usedFormats = new HashSet<BookFormatType>();

        foreach (BookFormatRequestDto format in formats)
        {
            if (!Enum.TryParse(format.FormatType, true, out BookFormatType parsedFormat))
            {
                throw new BadRequestException("Неправильний формат книги.");
            }

            if (!usedFormats.Add(parsedFormat))
            {
                throw new BadRequestException("Формати книги не повинні повторюватися.");
            }

            if (parsedFormat == BookFormatType.Paper ||
                parsedFormat == BookFormatType.Electronic)
            {
                if (format.PagesCount == null || format.PagesCount <= 0)
                {
                    throw new BadRequestException("Для паперової або електронної книги потрібно вказати кількість сторінок.");
                }
            }

            if (parsedFormat == BookFormatType.Audio)
            {
                if (format.DurationMinutes == null || format.DurationMinutes <= 0)
                {
                    throw new BadRequestException("Для аудіокниги потрібно вказати тривалість.");
                }
            }
        }
    }

    private static void ValidateSortParameters(
        string? sortBy,
        string? sortDirection)
    {
        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            string normalizedSortBy = sortBy.Trim().ToLower();

            string[] allowedSortFields =
            {
                "title",
                "author",
                "year",
                "genre"
            };

            if (!allowedSortFields.Contains(normalizedSortBy))
            {
                throw new BadRequestException("Неправильне поле для сортування.");
            }
        }

        if (!string.IsNullOrWhiteSpace(sortDirection))
        {
            string normalizedSortDirection = sortDirection.Trim().ToLower();

            if (normalizedSortDirection != "asc" &&
                normalizedSortDirection != "desc")
            {
                throw new BadRequestException("Напрям сортування має бути asc або desc.");
            }
        }
    }
}