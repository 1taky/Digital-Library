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

        CreateMap<CreateGenreRequestModel, CreateGenreRequestDto>();

        CreateMap<GenreResponseDto, GenreResponseModel>();

        CreateMap<CreateBookRequestModel, CreateBookRequestDto>();

        CreateMap<UpdateBookRequestModel, UpdateBookRequestDto>();

        CreateMap<BookFileResponseDto, BookFileResponseModel>();

        CreateMap<BookResponseDto, BookResponseModel>()
            .ForMember(destination => destination.CoverUrl, options => options.Ignore())
            .ForMember(destination => destination.DownloadUrl, options => options.Ignore())
            .ForMember(destination => destination.ListenUrl, options => options.Ignore());

        CreateMap<BookFilterRequestModel, BookFilterRequestDto>();

        CreateMap<BookFormatRequestModel, BookFormatRequestDto>();

        CreateMap<BookFormatResponseDto, BookFormatResponseModel>();
    }
}