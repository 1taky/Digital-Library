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

public class OrderServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly IMapper _mapper;
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _bookRepositoryMock = new Mock<IBookRepository>();

        _mapper = MapperHelper.CreateMapper();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Orders)
            .Returns(_orderRepositoryMock.Object);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Books)
            .Returns(_bookRepositoryMock.Object);

        _orderService = new OrderService(
            _unitOfWorkMock.Object,
            _mapper);
    }

    [Fact]
    public async Task RequestAsync_WhenBookHasNoPaperFormat_ShouldThrowBadRequestException()
    {
        CreateOrderRequestDto request = new CreateOrderRequestDto
        {
            BookId = 1,
            PhoneNumber = "+380991112233"
        };

        Book book = CreateElectronicBook();

        _bookRepositoryMock
            .Setup(repository => repository.GetByIdDetailedAsync(request.BookId))
            .ReturnsAsync(book);

        BadRequestException exception = await Assert.ThrowsAsync<BadRequestException>(() =>
            _orderService.RequestAsync(request, userId: 2));

        Assert.Equal("Замовити можна тільки паперову книгу.", exception.Message);
    }

    [Fact]
    public async Task RequestAsync_WhenPaperBookIsNotAvailable_ShouldThrowBadRequestException()
    {
        CreateOrderRequestDto request = new CreateOrderRequestDto
        {
            BookId = 1,
            PhoneNumber = "+380991112233"
        };

        Book book = CreatePaperBook(isAvailable: false);

        _bookRepositoryMock
            .Setup(repository => repository.GetByIdDetailedAsync(request.BookId))
            .ReturnsAsync(book);

        BadRequestException exception = await Assert.ThrowsAsync<BadRequestException>(() =>
            _orderService.RequestAsync(request, userId: 2));

        Assert.Equal("Паперова книга зараз недоступна.", exception.Message);
    }

    [Fact]
    public async Task RequestAsync_WhenBookHasActiveOrder_ShouldThrowBadRequestException()
    {
        CreateOrderRequestDto request = new CreateOrderRequestDto
        {
            BookId = 1,
            PhoneNumber = "+380991112233"
        };

        Book book = CreatePaperBook(isAvailable: true);

        _bookRepositoryMock
            .Setup(repository => repository.GetByIdDetailedAsync(request.BookId))
            .ReturnsAsync(book);

        _orderRepositoryMock
            .Setup(repository => repository.HasActiveOrderForBookAsync(book.Id))
            .ReturnsAsync(true);

        BadRequestException exception = await Assert.ThrowsAsync<BadRequestException>(() =>
            _orderService.RequestAsync(request, userId: 2));

        Assert.Equal("Для цієї книги вже існує активне замовлення або вона вже видана.", exception.Message);
    }

    [Fact]
    public async Task RequestAsync_WhenRequestIsValid_ShouldCreateOrderWithRequestedStatus()
    {
        CreateOrderRequestDto request = new CreateOrderRequestDto
        {
            BookId = 1,
            PhoneNumber = "+380991112233"
        };

        Book book = CreatePaperBook(isAvailable: true);

        Order? savedOrder = null;

        _bookRepositoryMock
            .Setup(repository => repository.GetByIdDetailedAsync(request.BookId))
            .ReturnsAsync(book);

        _orderRepositoryMock
            .Setup(repository => repository.HasActiveOrderForBookAsync(book.Id))
            .ReturnsAsync(false);

        _orderRepositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<Order>()))
            .Callback<Order>(order =>
            {
                order.Id = 1;
                order.Book = book;
                order.User = CreateUser();
                savedOrder = order;
            })
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync())
            .ReturnsAsync(1);

        _orderRepositoryMock
            .Setup(repository => repository.GetByIdDetailedAsync(1))
            .ReturnsAsync(() => savedOrder);

        OrderResponseDto result = await _orderService.RequestAsync(
            request,
            userId: 2);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Requested", result.Status);
        Assert.Equal("+380991112233", result.PhoneNumber);

        _orderRepositoryMock.Verify(repository =>
            repository.AddAsync(It.IsAny<Order>()),
            Times.Once);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task BorrowAsync_WhenOrderIsApproved_ShouldSetBorrowedStatusAndMakePaperUnavailable()
    {
        Book book = CreatePaperBook(isAvailable: true);

        Order order = CreateOrder(
            id: 1,
            status: OrderStatus.Approved,
            book: book);

        _orderRepositoryMock
            .Setup(repository => repository.GetByIdDetailedAsync(order.Id))
            .ReturnsAsync(order);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync())
            .ReturnsAsync(1);

        OrderResponseDto result = await _orderService.BorrowAsync(
            orderId: 1,
            managerId: 1);

        BookFormat paperFormat = book.Formats
            .First(format => format.FormatType == BookFormatType.Paper);

        Assert.Equal("Borrowed", result.Status);
        Assert.False(paperFormat.IsAvailable);
        Assert.NotNull(order.BorrowedAt);
        Assert.NotNull(order.DueDate);

        _orderRepositoryMock.Verify(repository =>
            repository.Update(order),
            Times.Once);

        _bookRepositoryMock.Verify(repository =>
            repository.Update(book),
            Times.Once);
    }

    [Fact]
    public async Task ReturnAsync_WhenOrderIsBorrowed_ShouldSetReturnedStatusAndMakePaperAvailable()
    {
        Book book = CreatePaperBook(isAvailable: false);

        Order order = CreateOrder(
            id: 1,
            status: OrderStatus.Borrowed,
            book: book);

        order.BorrowedAt = DateTime.UtcNow.AddDays(-10);
        order.DueDate = DateTime.UtcNow.AddMonths(2);

        _orderRepositoryMock
            .Setup(repository => repository.GetByIdDetailedAsync(order.Id))
            .ReturnsAsync(order);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync())
            .ReturnsAsync(1);

        OrderResponseDto result = await _orderService.ReturnAsync(
            orderId: 1,
            managerId: 1);

        BookFormat paperFormat = book.Formats
            .First(format => format.FormatType == BookFormatType.Paper);

        Assert.Equal("Returned", result.Status);
        Assert.True(paperFormat.IsAvailable);
        Assert.NotNull(order.ReturnedAt);

        _orderRepositoryMock.Verify(repository =>
            repository.Update(order),
            Times.Once);

        _bookRepositoryMock.Verify(repository =>
            repository.Update(book),
            Times.Once);
    }

    [Fact]
    public async Task BorrowAsync_WhenOrderIsRequested_ShouldThrowBadRequestException()
    {
        Book book = CreatePaperBook(isAvailable: true);

        Order order = CreateOrder(
            id: 1,
            status: OrderStatus.Requested,
            book: book);

        _orderRepositoryMock
            .Setup(repository => repository.GetByIdDetailedAsync(order.Id))
            .ReturnsAsync(order);

        BadRequestException exception = await Assert.ThrowsAsync<BadRequestException>(() =>
            _orderService.BorrowAsync(orderId: 1, managerId: 1));

        Assert.Equal("Видати можна тільки замовлення зі статусом Approved.", exception.Message);
    }

    [Fact]
    public async Task GetActiveOrderByBookIdAsync_WhenActiveOrderExists_ShouldReturnActiveStatus()
    {
        Book book = CreatePaperBook(isAvailable: true);

        Order order = CreateOrder(
            id: 5,
            status: OrderStatus.Requested,
            book: book);

        _bookRepositoryMock
            .Setup(repository => repository.GetByIdDetailedAsync(book.Id))
            .ReturnsAsync(book);

        _orderRepositoryMock
            .Setup(repository => repository.GetActiveByBookIdDetailedAsync(book.Id))
            .ReturnsAsync(order);

        BookOrderStatusResponseDto result =
            await _orderService.GetActiveOrderByBookIdAsync(book.Id);

        Assert.True(result.HasActiveOrder);
        Assert.Equal(book.Id, result.BookId);
        Assert.Equal(order.Id, result.OrderId);
        Assert.Equal("Requested", result.Status);
    }

    [Fact]
    public async Task GetActiveOrderByBookIdAsync_WhenActiveOrderDoesNotExist_ShouldReturnFalse()
    {
        Book book = CreatePaperBook(isAvailable: true);

        _bookRepositoryMock
            .Setup(repository => repository.GetByIdDetailedAsync(book.Id))
            .ReturnsAsync(book);

        _orderRepositoryMock
            .Setup(repository => repository.GetActiveByBookIdDetailedAsync(book.Id))
            .ReturnsAsync((Order?)null);

        BookOrderStatusResponseDto result =
            await _orderService.GetActiveOrderByBookIdAsync(book.Id);

        Assert.False(result.HasActiveOrder);
        Assert.Equal(book.Id, result.BookId);
        Assert.Null(result.OrderId);
        Assert.Null(result.Status);
    }

    private static Book CreatePaperBook(bool isAvailable)
    {
        Genre genre = new Genre
        {
            Id = 1,
            Name = "Fantasy"
        };

        Book book = new Book
        {
            Id = 1,
            Title = "Paper Test Book",
            Author = "Test Author",
            Description = "Book for order testing.",
            GenreId = genre.Id,
            Genre = genre,
            Language = "English",
            PublicationYear = 2021,
            Formats = new List<BookFormat>
            {
                new BookFormat
                {
                    Id = 1,
                    BookId = 1,
                    FormatType = BookFormatType.Paper,
                    IsAvailable = isAvailable,
                    PagesCount = 250,
                    DurationMinutes = null
                }
            },
            Files = new List<BookFile>()
        };

        return book;
    }

    private static Book CreateElectronicBook()
    {
        Genre genre = new Genre
        {
            Id = 1,
            Name = "Fantasy"
        };

        Book book = new Book
        {
            Id = 1,
            Title = "Electronic Book",
            Author = "Test Author",
            Description = "Electronic only.",
            GenreId = genre.Id,
            Genre = genre,
            Language = "English",
            PublicationYear = 2022,
            Formats = new List<BookFormat>
            {
                new BookFormat
                {
                    Id = 1,
                    BookId = 1,
                    FormatType = BookFormatType.Electronic,
                    IsAvailable = true,
                    PagesCount = 300,
                    DurationMinutes = null
                }
            },
            Files = new List<BookFile>()
        };

        return book;
    }

    private static User CreateUser()
    {
        User user = new User
        {
            Id = 2,
            FullName = "Test User",
            Email = "user@test.com",
            NormalizedEmail = "USER@TEST.COM",
            PasswordHash = "hashed-password",
            Role = Role.User,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        return user;
    }

    private static Order CreateOrder(
        int id,
        OrderStatus status,
        Book book)
    {
        Order order = new Order
        {
            Id = id,
            UserId = 2,
            User = CreateUser(),
            BookId = book.Id,
            Book = book,
            PhoneNumber = "+380991112233",
            Status = status,
            CreatedAt = DateTime.UtcNow
        };

        return order;
    }
}