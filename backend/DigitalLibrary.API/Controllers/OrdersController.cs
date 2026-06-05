using System.Security.Claims;
using AutoMapper;
using DigitalLibrary.API.Models.Requests;
using DigitalLibrary.API.Models.Responses;
using DigitalLibrary.BLL.DTOs.Requests;
using DigitalLibrary.BLL.DTOs.Responses;
using DigitalLibrary.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalLibrary.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public OrdersController(
        IOrderService orderService,
        IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }

    [HttpPost("request")]
    [Authorize]
    public async Task<ActionResult<OrderResponseModel>> RequestOrder(
        CreateOrderRequestModel model)
    {
        int userId = GetCurrentUserId();

        CreateOrderRequestDto requestDto =
            _mapper.Map<CreateOrderRequestDto>(model);

        OrderResponseDto responseDto =
            await _orderService.RequestAsync(requestDto, userId);

        OrderResponseModel responseModel =
            _mapper.Map<OrderResponseModel>(responseDto);

        return Ok(responseModel);
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<ActionResult<List<OrderResponseModel>>> GetMyOrders()
    {
        int userId = GetCurrentUserId();

        List<OrderResponseDto> responseDto =
            await _orderService.GetMyOrdersAsync(userId);

        List<OrderResponseModel> responseModel =
            _mapper.Map<List<OrderResponseModel>>(responseDto);

        return Ok(responseModel);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<List<OrderResponseModel>>> GetAll()
    {
        List<OrderResponseDto> responseDto =
            await _orderService.GetAllAsync();

        List<OrderResponseModel> responseModel =
            _mapper.Map<List<OrderResponseModel>>(responseDto);

        return Ok(responseModel);
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<OrderResponseModel>> GetById(int id)
    {
        int currentUserId = GetCurrentUserId();

        string currentUserRole = GetCurrentUserRole();

        OrderResponseDto responseDto =
            await _orderService.GetByIdAsync(
                id,
                currentUserId,
                currentUserRole);

        OrderResponseModel responseModel =
            _mapper.Map<OrderResponseModel>(responseDto);

        return Ok(responseModel);
    }

    [HttpGet("book/{bookId:int}/active")]
    [AllowAnonymous]
    public async Task<ActionResult<BookOrderStatusResponseModel>> GetActiveOrderByBookId(
    int bookId)
    {
        BookOrderStatusResponseDto responseDto =
            await _orderService.GetActiveOrderByBookIdAsync(bookId);

        BookOrderStatusResponseModel responseModel =
            _mapper.Map<BookOrderStatusResponseModel>(responseDto);

        return Ok(responseModel);
    }

    [HttpPatch("{id:int}/approve")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<OrderResponseModel>> Approve(int id)
    {
        int managerId = GetCurrentUserId();

        OrderResponseDto responseDto =
            await _orderService.ApproveAsync(id, managerId);

        OrderResponseModel responseModel =
            _mapper.Map<OrderResponseModel>(responseDto);

        return Ok(responseModel);
    }

    [HttpPatch("{id:int}/borrow")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<OrderResponseModel>> Borrow(int id)
    {
        int managerId = GetCurrentUserId();

        OrderResponseDto responseDto =
            await _orderService.BorrowAsync(id, managerId);

        OrderResponseModel responseModel =
            _mapper.Map<OrderResponseModel>(responseDto);

        return Ok(responseModel);
    }

    [HttpPatch("{id:int}/return")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<OrderResponseModel>> Return(int id)
    {
        int managerId = GetCurrentUserId();

        OrderResponseDto responseDto =
            await _orderService.ReturnAsync(id, managerId);

        OrderResponseModel responseModel =
            _mapper.Map<OrderResponseModel>(responseDto);

        return Ok(responseModel);
    }

    [HttpPatch("{id:int}/reject")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<OrderResponseModel>> Reject(int id)
    {
        int managerId = GetCurrentUserId();

        OrderResponseDto responseDto =
            await _orderService.RejectAsync(id, managerId);

        OrderResponseModel responseModel =
            _mapper.Map<OrderResponseModel>(responseDto);

        return Ok(responseModel);
    }

    [HttpGet("overdue")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<List<OrderResponseModel>>> GetOverdue()
    {
        List<OrderResponseDto> responseDto =
            await _orderService.GetOverdueAsync();

        List<OrderResponseModel> responseModel =
            _mapper.Map<List<OrderResponseModel>>(responseDto);

        return Ok(responseModel);
    }

    private int GetCurrentUserId()
    {
        string? userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userIdValue))
        {
            throw new UnauthorizedAccessException("Користувач не авторизований.");
        }

        return int.Parse(userIdValue);
    }

    private string GetCurrentUserRole()
    {
        string? roleValue = User.FindFirstValue(ClaimTypes.Role);

        if (string.IsNullOrWhiteSpace(roleValue))
        {
            throw new UnauthorizedAccessException("Роль користувача не знайдено.");
        }

        return roleValue;
    }
}