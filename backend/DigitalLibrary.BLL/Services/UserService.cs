using AutoMapper;
using DigitalLibrary.BLL.DTOs.Requests;
using DigitalLibrary.BLL.DTOs.Responses;
using DigitalLibrary.BLL.Exceptions;
using DigitalLibrary.BLL.Interfaces;
using DigitalLibrary.DAL.Entities;
using DigitalLibrary.DAL.Enums;
using DigitalLibrary.DAL.Interfaces;

namespace DigitalLibrary.BLL.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<UserManagementResponseDto>> GetAllAsync()
    {
        List<User> users = await _unitOfWork.Users.GetAllAsync();

        return _mapper.Map<List<UserManagementResponseDto>>(users);
    }

    public async Task<UserManagementResponseDto> GetByIdAsync(int id)
    {
        User user = await GetUserOrThrowAsync(id);

        return _mapper.Map<UserManagementResponseDto>(user);
    }

    public async Task<UserManagementResponseDto> UpdateRoleAsync(
        int id,
        UpdateUserRoleRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Role))
        {
            throw new BadRequestException("Роль користувача є обов'язковою.");
        }

        bool roleIsValid = Enum.TryParse(
            request.Role,
            true,
            out Role parsedRole);

        if (!roleIsValid)
        {
            throw new BadRequestException("Неправильна роль користувача.");
        }

        User user = await GetUserOrThrowAsync(id);

        user.Role = parsedRole;

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<UserManagementResponseDto>(user);
    }

    public async Task<UserManagementResponseDto> BlockAsync(int id)
    {
        User user = await GetUserOrThrowAsync(id);

        if (!user.IsActive)
        {
            throw new BadRequestException("Користувач уже заблокований.");
        }

        user.IsActive = false;

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<UserManagementResponseDto>(user);
    }

    public async Task<UserManagementResponseDto> UnblockAsync(int id)
    {
        User user = await GetUserOrThrowAsync(id);

        if (user.IsActive)
        {
            throw new BadRequestException("Користувач уже активний.");
        }

        user.IsActive = true;

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<UserManagementResponseDto>(user);
    }

    private async Task<User> GetUserOrThrowAsync(int id)
    {
        User? user = await _unitOfWork.Users.GetByIdAsync(id);

        if (user == null)
        {
            throw new NotFoundException("Користувача не знайдено.");
        }

        return user;
    }
}