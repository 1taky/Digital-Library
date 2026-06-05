using AutoMapper;
using DigitalLibrary.BLL.DTOs.Requests;
using DigitalLibrary.BLL.DTOs.Responses;
using DigitalLibrary.BLL.Exceptions;
using DigitalLibrary.BLL.Services;
using DigitalLibrary.DAL.Entities;
using DigitalLibrary.DAL.Enums;
using DigitalLibrary.DAL.Interfaces;
using DigitalLibrary.Tests.Helpers;
using Moq;

namespace DigitalLibrary.Tests.Services;

public class BookServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly Mock<IGenreRepository> _genreRepositoryMock;
    private readonly IMapper _mapper;
    private readonly BookService _bookService;

    public BookServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _bookRepositoryMock = new Mock<IBookRepository>();
        _genreRepositoryMock = new Mock<IGenreRepository>();

        _mapper = MapperHelper.CreateMapper();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Books)
            .Returns(_bookRepositoryMock.Object);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Genres)
            .Returns(_genreRepositoryMock.Object);

        _bookService = new BookService(
            _unitOfWorkMock.Object,
            _mapper);
    }

    [Fact]
    public async Task CreateAsync_WhenGenreDoesNotExist_ShouldThrowNotFoundException()
    {
        CreateBookRequestDto request = CreateValidBookRequest();

        _genreRepositoryMock
            .Setup(repository => repository.GetByNameAsync(request.GenreName))
            .ReturnsAsync((Genre?)null);

        NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            _bookService.CreateAsync(request));

        Assert.Equal("Жанр не знайдено.", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_WhenBookDuplicateExists_ShouldThrowBadRequestException()
    {
        CreateBookRequestDto request = CreateValidBookRequest();

        Genre genre = new Genre
        {
            Id = 1,
            Name = "Fantasy"
        };

        _genreRepositoryMock
            .Setup(repository => repository.GetByNameAsync(request.GenreName))
            .ReturnsAsync(genre);

        _bookRepositoryMock
            .Setup(repository => repository.ExistsDuplicateAsync(
                request.Title,
                request.Author,
                request.Language,
                request.PublicationYear))
            .ReturnsAsync(true);

        BadRequestException exception = await Assert.ThrowsAsync<BadRequestException>(() =>
            _bookService.CreateAsync(request));

        Assert.Equal("Така книга вже існує.", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_WhenRequestIsValid_ShouldCreateBook()
    {
        CreateBookRequestDto request = CreateValidBookRequest();

        Genre genre = new Genre
        {
            Id = 1,
            Name = "Fantasy"
        };

        Book? savedBook = null;

        _genreRepositoryMock
            .Setup(repository => repository.GetByNameAsync(request.GenreName))
            .ReturnsAsync(genre);

        _bookRepositoryMock
            .Setup(repository => repository.ExistsDuplicateAsync(
                request.Title,
                request.Author,
                request.Language,
                request.PublicationYear))
            .ReturnsAsync(false);

        _bookRepositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<Book>()))
            .Callback<Book>(book =>
            {
                book.Id = 1;
                book.GenreId = genre.Id;
                book.Genre = genre;
                savedBook = book;
            })
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync())
            .ReturnsAsync(1);

        _bookRepositoryMock
            .Setup(repository => repository.GetByIdDetailedAsync(1))
            .ReturnsAsync(() => savedBook);

        BookResponseDto result = await _bookService.CreateAsync(request);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Atomic Habits", result.Title);
        Assert.Equal("James Clear", result.Author);
        Assert.Equal("Fantasy", result.GenreName);
        Assert.Equal(3, result.Formats.Count);

        _bookRepositoryMock.Verify(repository =>
            repository.AddAsync(It.IsAny<Book>()),
            Times.Once);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WhenFormatsAreDuplicated_ShouldThrowBadRequestException()
    {
        CreateBookRequestDto request = CreateValidBookRequest();

        request.Formats = new List<BookFormatRequestDto>
        {
            new BookFormatRequestDto
            {
                FormatType = "Paper",
                PagesCount = 320,
                DurationMinutes = null
            },
            new BookFormatRequestDto
            {
                FormatType = "Paper",
                PagesCount = 320,
                DurationMinutes = null
            }
        };

        BadRequestException exception = await Assert.ThrowsAsync<BadRequestException>(() =>
            _bookService.CreateAsync(request));

        Assert.Equal("Формати книги не повинні повторюватися.", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_WhenAudioHasNoDuration_ShouldThrowBadRequestException()
    {
        CreateBookRequestDto request = CreateValidBookRequest();

        request.Formats = new List<BookFormatRequestDto>
        {
            new BookFormatRequestDto
            {
                FormatType = "Audio",
                PagesCount = null,
                DurationMinutes = null
            }
        };

        BadRequestException exception = await Assert.ThrowsAsync<BadRequestException>(() =>
            _bookService.CreateAsync(request));

        Assert.Equal("Для аудіокниги потрібно вказати тривалість.", exception.Message);
    }

    private static CreateBookRequestDto CreateValidBookRequest()
    {
        CreateBookRequestDto request = new CreateBookRequestDto
        {
            Title = "Atomic Habits",
            Author = "James Clear",
            Description = "Book about habits.",
            GenreName = "Fantasy",
            Language = "English",
            PublicationYear = 2018,
            Formats = new List<BookFormatRequestDto>
            {
                new BookFormatRequestDto
                {
                    FormatType = "Paper",
                    PagesCount = 320,
                    DurationMinutes = null
                },
                new BookFormatRequestDto
                {
                    FormatType = "Electronic",
                    PagesCount = 320,
                    DurationMinutes = null
                },
                new BookFormatRequestDto
                {
                    FormatType = "Audio",
                    PagesCount = null,
                    DurationMinutes = 480
                }
            }
        };

        return request;
    }
}