using AutoMapper;
using DigitalLibrary.API.Models.Requests;
using DigitalLibrary.API.Models.Responses;
using DigitalLibrary.BLL.DTOs.Requests;
using DigitalLibrary.BLL.DTOs.Responses;

namespace DigitalLibrary.API.Mapping;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<RegisterRequestModel, RegisterRequestDto>();

        CreateMap<LoginRequestModel, LoginRequestDto>();

        CreateMap<AuthResponseDto, AuthResponseModel>();

        CreateMap<UserResponseDto, UserResponseModel>();
    }
}