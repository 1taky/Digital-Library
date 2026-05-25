using AutoMapper;
using DigitalLibrary.BLL.DTOs.Requests;
using DigitalLibrary.BLL.DTOs.Responses;
using DigitalLibrary.BLL.Exceptions;
using DigitalLibrary.BLL.Interfaces;
using DigitalLibrary.DAL.Entities;
using DigitalLibrary.DAL.Enums;
using DigitalLibrary.DAL.Interfaces;

namespace DigitalLibrary.BLL.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;

    public AuthService(
        IUnitOfWork unitOfWork,
        IJwtService jwtService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _mapper = mapper;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        ValidateRegisterRequest(request);

        bool emailExists = await _unitOfWork.Users
            .ExistsByEmailAsync(request.Email);

        if (emailExists)
        {
            throw new BadRequestException("Користувач з таким email вже існує.");
        }

        User user = _mapper.Map<User>(request);

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        user.Role = Role.User;
        user.IsActive = true;
        user.CreatedAt = DateTime.UtcNow;

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        string token = _jwtService.GenerateToken(user);

        AuthResponseDto response = _mapper.Map<AuthResponseDto>(user);
        response.Token = token;

        return response;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        ValidateLoginRequest(request);

        User? user = await _unitOfWork.Users
            .GetByEmailAsync(request.Email);

        if (user == null)
        {
            throw new UnauthorizedException("Неправильний email або пароль.");
        }

        if (!user.IsActive)
        {
            throw new ForbiddenException("Обліковий запис заблоковано.");
        }

        bool passwordIsValid = BCrypt.Net.BCrypt.Verify(
            request.Password,
            user.PasswordHash);

        if (!passwordIsValid)
        {
            throw new UnauthorizedException("Неправильний email або пароль.");
        }

        string token = _jwtService.GenerateToken(user);

        AuthResponseDto response = _mapper.Map<AuthResponseDto>(user);
        response.Token = token;

        return response;
    }

    public async Task<UserResponseDto> GetCurrentUserAsync(int userId)
    {
        User? user = await _unitOfWork.Users.GetByIdAsync(userId);

        if (user == null)
        {
            throw new UnauthorizedException("Користувача не знайдено.");
        }

        if (!user.IsActive)
        {
            throw new ForbiddenException("Обліковий запис заблоковано.");
        }

        return _mapper.Map<UserResponseDto>(user);
    }

    private static void ValidateRegisterRequest(RegisterRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName))
        {
            throw new BadRequestException("Ім'я користувача є обов'язковим.");
        }

        if (request.FullName.Length < 2)
        {
            throw new BadRequestException("Ім'я має містити мінімум 2 символи.");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            throw new BadRequestException("Email є обов'язковим.");
        }

        if (!request.Email.Contains('@'))
        {
            throw new BadRequestException("Email має неправильний формат.");
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new BadRequestException("Пароль є обов'язковим.");
        }

        if (request.Password.Length < 6)
        {
            throw new BadRequestException("Пароль має містити мінімум 6 символів.");
        }
    }

    private static void ValidateLoginRequest(LoginRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            throw new BadRequestException("Email є обов'язковим.");
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new BadRequestException("Пароль є обов'язковим.");
        }
    }
}