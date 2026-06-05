using DigitalLibrary.BLL.DTOs.Requests;
using DigitalLibrary.BLL.DTOs.Responses;

namespace DigitalLibrary.BLL.Interfaces;

public interface IUserService
{
    Task<List<UserManagementResponseDto>> GetAllAsync();

    Task<UserManagementResponseDto> GetByIdAsync(int id);

    Task<UserManagementResponseDto> UpdateRoleAsync(
        int id,
        UpdateUserRoleRequestDto request);

    Task<UserManagementResponseDto> BlockAsync(int id);

    Task<UserManagementResponseDto> UnblockAsync(int id);
}