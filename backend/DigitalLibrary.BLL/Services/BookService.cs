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
        List<Book> books = await _unitOfWork.Books.GetAllWithGenreAsync();

        return _mapper.Map<List<BookResponseDto>>(books);
    }

    public async Task<BookResponseDto> GetByIdAsync(int id)
    {
        Book? book = await _unitOfWork.Books.GetByIdWithGenreAsync(id);

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
            request.BookType,
            request.GenreId,
            request.Language,
            request.PublicationYear,
            request.PagesCount,
            request.DurationMinutes);

        Genre? genre = await _unitOfWork.Genres.GetByIdAsync(request.GenreId);

        if (genre == null)
        {
            throw new NotFoundException("Жанр не знайдено.");
        }

        Book book = _mapper.Map<Book>(request);

        book.IsAvailable = true;
        book.CreatedAt = DateTime.UtcNow;

        await _unitOfWork.Books.AddAsync(book);
        await _unitOfWork.SaveChangesAsync();

        Book createdBook = await _unitOfWork.Books.GetByIdWithGenreAsync(book.Id)
            ?? throw new NotFoundException("Книгу не знайдено після створення.");

        return _mapper.Map<BookResponseDto>(createdBook);
    }

    public async Task<BookResponseDto> UpdateAsync(int id, UpdateBookRequestDto request)
    {
        ValidateBookRequest(
            request.Title,
            request.Author,
            request.Description,
            request.BookType,
            request.GenreId,
            request.Language,
            request.PublicationYear,
            request.PagesCount,
            request.DurationMinutes);

        Book? book = await _unitOfWork.Books.GetByIdWithGenreAsync(id);

        if (book == null)
        {
            throw new NotFoundException("Книгу не знайдено.");
        }

        Genre? genre = await _unitOfWork.Genres.GetByIdAsync(request.GenreId);

        if (genre == null)
        {
            throw new NotFoundException("Жанр не знайдено.");
        }

        _mapper.Map(request, book);

        _unitOfWork.Books.Update(book);
        await _unitOfWork.SaveChangesAsync();

        Book updatedBook = await _unitOfWork.Books.GetByIdWithGenreAsync(book.Id)
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

    private static void ValidateBookRequest(
        string title,
        string author,
        string description,
        string bookType,
        int genreId,
        string language,
        int publicationYear,
        int? pagesCount,
        int? durationMinutes)
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

        if (!Enum.TryParse(bookType, true, out BookType parsedBookType))
        {
            throw new BadRequestException("Неправильний тип книги.");
        }

        if (genreId <= 0)
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

        if (parsedBookType == BookType.Audio)
        {
            if (durationMinutes == null || durationMinutes <= 0)
            {
                throw new BadRequestException("Для аудіокниги потрібно вказати тривалість.");
            }
        }
        else
        {
            if (pagesCount == null || pagesCount <= 0)
            {
                throw new BadRequestException("Для паперової або електронної книги потрібно вказати кількість сторінок.");
            }
        }
    }

    public async Task<List<BookResponseDto>> GetFilteredAsync(BookFilterRequestDto request)
    {
        BookType? parsedBookType = null;

        if (!string.IsNullOrWhiteSpace(request.BookType))
        {
            bool isValidBookType = Enum.TryParse(
                request.BookType,
                true,
                out BookType bookType);

            if (!isValidBookType)
            {
                throw new BadRequestException("Неправильний тип книги.");
            }

            parsedBookType = bookType;
        }

        ValidateSortParameters(request.SortBy, request.SortDirection);

        List<Book> books = await _unitOfWork.Books.GetFilteredAsync(
            request.Search,
            request.GenreId,
            parsedBookType,
            request.SortBy,
            request.SortDirection);

        return _mapper.Map<List<BookResponseDto>>(books);
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
            "type",
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