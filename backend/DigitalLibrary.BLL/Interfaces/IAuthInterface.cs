using DigitalLibrary.BLL.DTOs.Requests;
using DigitalLibrary.BLL.DTOs.Responses;

namespace DigitalLibrary.BLL.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);

    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);

    Task<UserResponseDto> GetCurrentUserAsync(int userId);
}