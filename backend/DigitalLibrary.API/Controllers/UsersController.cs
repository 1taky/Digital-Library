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
[Route("api/users")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UsersController(
        IUserService userService,
        IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserManagementResponseModel>>> GetAll()
    {
        List<UserManagementResponseDto> responseDto =
            await _userService.GetAllAsync();

        List<UserManagementResponseModel> responseModel =
            _mapper.Map<List<UserManagementResponseModel>>(responseDto);

        return Ok(responseModel);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserManagementResponseModel>> GetById(int id)
    {
        UserManagementResponseDto responseDto =
            await _userService.GetByIdAsync(id);

        UserManagementResponseModel responseModel =
            _mapper.Map<UserManagementResponseModel>(responseDto);

        return Ok(responseModel);
    }

    [HttpPatch("{id:int}/role")]
    public async Task<ActionResult<UserManagementResponseModel>> UpdateRole(
        int id,
        UpdateUserRoleRequestModel model)
    {
        UpdateUserRoleRequestDto requestDto =
            _mapper.Map<UpdateUserRoleRequestDto>(model);

        UserManagementResponseDto responseDto =
            await _userService.UpdateRoleAsync(id, requestDto);

        UserManagementResponseModel responseModel =
            _mapper.Map<UserManagementResponseModel>(responseDto);

        return Ok(responseModel);
    }

    [HttpPatch("{id:int}/block")]
    public async Task<ActionResult<UserManagementResponseModel>> Block(int id)
    {
        UserManagementResponseDto responseDto =
            await _userService.BlockAsync(id);

        UserManagementResponseModel responseModel =
            _mapper.Map<UserManagementResponseModel>(responseDto);

        return Ok(responseModel);
    }

    [HttpPatch("{id:int}/unblock")]
    public async Task<ActionResult<UserManagementResponseModel>> Unblock(int id)
    {
        UserManagementResponseDto responseDto =
            await _userService.UnblockAsync(id);

        UserManagementResponseModel responseModel =
            _mapper.Map<UserManagementResponseModel>(responseDto);

        return Ok(responseModel);
    }
}