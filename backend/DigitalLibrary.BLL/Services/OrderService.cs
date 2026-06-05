using AutoMapper;
using DigitalLibrary.BLL.DTOs.Requests;
using DigitalLibrary.BLL.DTOs.Responses;
using DigitalLibrary.BLL.Exceptions;
using DigitalLibrary.BLL.Interfaces;
using DigitalLibrary.DAL.Entities;
using DigitalLibrary.DAL.Enums;
using DigitalLibrary.DAL.Interfaces;

namespace DigitalLibrary.BLL.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OrderResponseDto> RequestAsync(
        CreateOrderRequestDto request,
        int userId)
    {
        ValidateCreateRequest(request);

        Book book = await GetBookOrThrowAsync(request.BookId);

        BookFormat paperFormat = GetPaperFormatOrThrow(book);

        if (!paperFormat.IsAvailable)
        {
            throw new BadRequestException("Паперова книга зараз недоступна.");
        }

        bool hasActiveOrder = await _unitOfWork.Orders
            .HasActiveOrderForBookAsync(book.Id);

        if (hasActiveOrder)
        {
            throw new BadRequestException("Для цієї книги вже існує активне замовлення або вона вже видана.");
        }

        Order order = new Order
        {
            UserId = userId,
            BookId = book.Id,
            PhoneNumber = request.PhoneNumber.Trim(),
            Status = OrderStatus.Requested,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        Order createdOrder = await _unitOfWork.Orders.GetByIdDetailedAsync(order.Id)
            ?? throw new NotFoundException("Замовлення не знайдено після створення.");

        return _mapper.Map<OrderResponseDto>(createdOrder);
    }

    public async Task<List<OrderResponseDto>> GetMyOrdersAsync(int userId)
    {
        List<Order> orders = await _unitOfWork.Orders
            .GetByUserIdDetailedAsync(userId);

        await UpdateOverdueOrdersIfNeededAsync(orders);

        return _mapper.Map<List<OrderResponseDto>>(orders);
    }

    public async Task<List<OrderResponseDto>> GetAllAsync()
    {
        List<Order> orders = await _unitOfWork.Orders
            .GetAllDetailedAsync();

        await UpdateOverdueOrdersIfNeededAsync(orders);

        return _mapper.Map<List<OrderResponseDto>>(orders);
    }

    public async Task<OrderResponseDto> GetByIdAsync(
    int orderId,
    int currentUserId,
    string currentUserRole)
    {
        Order order = await GetOrderOrThrowAsync(orderId);

        bool isAdminOrManager =
            string.Equals(currentUserRole, "Admin", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(currentUserRole, "Manager", StringComparison.OrdinalIgnoreCase);

        bool isOwner = order.UserId == currentUserId;

        if (!isAdminOrManager && !isOwner)
        {
            throw new ForbiddenException("Немає доступу до цього замовлення.");
        }

        if (order.Status == OrderStatus.Borrowed &&
            order.DueDate.HasValue &&
            order.DueDate.Value < DateTime.UtcNow)
        {
            order.Status = OrderStatus.Overdue;

            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();

            order = await GetOrderOrThrowAsync(orderId);
        }

        return _mapper.Map<OrderResponseDto>(order);
    }
    public async Task<OrderResponseDto> ApproveAsync(int orderId, int managerId)
    {
        Order order = await GetOrderOrThrowAsync(orderId);

        if (order.Status != OrderStatus.Requested)
        {
            throw new BadRequestException("Підтвердити можна тільки замовлення зі статусом Requested.");
        }

        order.Status = OrderStatus.Approved;
        order.ManagerId = managerId;

        _unitOfWork.Orders.Update(order);
        await _unitOfWork.SaveChangesAsync();

        Order updatedOrder = await GetOrderOrThrowAsync(order.Id);

        return _mapper.Map<OrderResponseDto>(updatedOrder);
    }

    public async Task<OrderResponseDto> BorrowAsync(int orderId, int managerId)
    {
        Order order = await GetOrderOrThrowAsync(orderId);

        if (order.Status != OrderStatus.Approved)
        {
            throw new BadRequestException("Видати можна тільки замовлення зі статусом Approved.");
        }

        BookFormat paperFormat = GetPaperFormatOrThrow(order.Book);

        if (!paperFormat.IsAvailable)
        {
            throw new BadRequestException("Паперова книга вже недоступна.");
        }

        DateTime currentDate = DateTime.UtcNow;

        order.Status = OrderStatus.Borrowed;
        order.ManagerId = managerId;
        order.BorrowedAt = currentDate;
        order.DueDate = currentDate.AddDays(14);

        paperFormat.IsAvailable = false;

        _unitOfWork.Orders.Update(order);
        _unitOfWork.Books.Update(order.Book);

        await _unitOfWork.SaveChangesAsync();

        Order updatedOrder = await GetOrderOrThrowAsync(order.Id);

        return _mapper.Map<OrderResponseDto>(updatedOrder);
    }

    public async Task<OrderResponseDto> ReturnAsync(int orderId, int managerId)
    {
        Order order = await GetOrderOrThrowAsync(orderId);

        if (order.Status != OrderStatus.Borrowed &&
            order.Status != OrderStatus.Overdue)
        {
            throw new BadRequestException("Повернути можна тільки книгу зі статусом Borrowed або Overdue.");
        }

        BookFormat paperFormat = GetPaperFormatOrThrow(order.Book);

        order.Status = OrderStatus.Returned;
        order.ManagerId = managerId;
        order.ReturnedAt = DateTime.UtcNow;

        paperFormat.IsAvailable = true;

        _unitOfWork.Orders.Update(order);
        _unitOfWork.Books.Update(order.Book);

        await _unitOfWork.SaveChangesAsync();

        Order updatedOrder = await GetOrderOrThrowAsync(order.Id);

        return _mapper.Map<OrderResponseDto>(updatedOrder);
    }

    public async Task<OrderResponseDto> RejectAsync(int orderId, int managerId)
    {
        Order order = await GetOrderOrThrowAsync(orderId);

        if (order.Status != OrderStatus.Requested)
        {
            throw new BadRequestException("Відхилити можна тільки замовлення зі статусом Requested.");
        }

        order.Status = OrderStatus.Rejected;
        order.ManagerId = managerId;

        _unitOfWork.Orders.Update(order);
        await _unitOfWork.SaveChangesAsync();

        Order updatedOrder = await GetOrderOrThrowAsync(order.Id);

        return _mapper.Map<OrderResponseDto>(updatedOrder);
    }

    private async Task<Book> GetBookOrThrowAsync(int bookId)
    {
        Book? book = await _unitOfWork.Books.GetByIdDetailedAsync(bookId);

        if (book == null)
        {
            throw new NotFoundException("Книгу не знайдено.");
        }

        return book;
    }

    private async Task<Order> GetOrderOrThrowAsync(int orderId)
    {
        Order? order = await _unitOfWork.Orders.GetByIdDetailedAsync(orderId);

        if (order == null)
        {
            throw new NotFoundException("Замовлення не знайдено.");
        }

        return order;
    }

    private static BookFormat GetPaperFormatOrThrow(Book book)
    {
        BookFormat? paperFormat = book.Formats
            .FirstOrDefault(format => format.FormatType == BookFormatType.Paper);

        if (paperFormat == null)
        {
            throw new BadRequestException("Замовити можна тільки паперову книгу.");
        }

        return paperFormat;
    }

    private static void ValidateCreateRequest(CreateOrderRequestDto request)
    {
        if (request.BookId <= 0)
        {
            throw new BadRequestException("Книга є обов'язковою.");
        }

        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            throw new BadRequestException("Номер телефону є обов'язковим.");
        }

        if (request.PhoneNumber.Trim().Length < 7)
        {
            throw new BadRequestException("Номер телефону має неправильний формат.");
        }
    }

    private async Task UpdateOverdueOrdersIfNeededAsync(List<Order> orders)
    {
        bool hasChanges = false;

        foreach (Order order in orders)
        {
            if (order.Status == OrderStatus.Borrowed &&
                order.DueDate.HasValue &&
                order.DueDate.Value < DateTime.UtcNow)
            {
                order.Status = OrderStatus.Overdue;

                _unitOfWork.Orders.Update(order);

                hasChanges = true;
            }
        }

        if (hasChanges)
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task<List<OrderResponseDto>> GetOverdueAsync()
    {
        List<Order> allOrders = await _unitOfWork.Orders.GetAllDetailedAsync();

        await UpdateOverdueOrdersIfNeededAsync(allOrders);

        List<Order> overdueOrders = await _unitOfWork.Orders.GetOverdueDetailedAsync();

        return _mapper.Map<List<OrderResponseDto>>(overdueOrders);
    }
}