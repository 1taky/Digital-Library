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
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public AuthController(
        IAuthService authService,
        IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseModel>> Register(RegisterRequestModel model)
    {
        RegisterRequestDto requestDto = _mapper.Map<RegisterRequestDto>(model);

        AuthResponseDto responseDto = await _authService.RegisterAsync(requestDto);

        AuthResponseModel responseModel = _mapper.Map<AuthResponseModel>(responseDto);

        return Ok(responseModel);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseModel>> Login(LoginRequestModel model)
    {
        LoginRequestDto requestDto = _mapper.Map<LoginRequestDto>(model);

        AuthResponseDto responseDto = await _authService.LoginAsync(requestDto);

        AuthResponseModel responseModel = _mapper.Map<AuthResponseModel>(responseDto);

        return Ok(responseModel);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserResponseModel>> GetCurrentUser()
    {
        string? userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userIdValue))
        {
            return Unauthorized();
        }

        int userId = int.Parse(userIdValue);

        UserResponseDto responseDto = await _authService.GetCurrentUserAsync(userId);

        UserResponseModel responseModel = _mapper.Map<UserResponseModel>(responseDto);

        return Ok(responseModel);
    }
}