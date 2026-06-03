using AutoMapper;
using DigitalLibrary.BLL.DTOs.Requests;
using DigitalLibrary.BLL.DTOs.Responses;
using DigitalLibrary.DAL.Entities;
using DigitalLibrary.DAL.Enums;

namespace DigitalLibrary.BLL.Mapping;

public class BllMappingProfile : Profile
{
    public BllMappingProfile()
    {
        CreateMap<RegisterRequestDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Trim()))
            .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.Trim().ToUpper()))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName.Trim()));

        CreateMap<User, AuthResponseDto>()
            .ForMember(dest => dest.Token, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

        CreateMap<User, UserResponseDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

        CreateMap<CreateGenreRequestDto, Genre>()
        .ForMember(destination => destination.Id, options => options.Ignore())
        .ForMember(
            destination => destination.Name,
            options => options.MapFrom(source => source.Name.Trim()));

        CreateMap<Genre, GenreResponseDto>();


        CreateMap<CreateBookRequestDto, Book>()
    .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.BookType, opt => opt.MapFrom(src => Enum.Parse<BookType>(src.BookType, true)))
    .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Trim()))
    .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.Trim()))
    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Trim()))
    .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language.Trim()))
    .ForMember(dest => dest.IsAvailable, opt => opt.Ignore())
    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
    .ForMember(dest => dest.Genre, opt => opt.Ignore());

        CreateMap<UpdateBookRequestDto, Book>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.BookType, opt => opt.MapFrom(src => Enum.Parse<BookType>(src.BookType, true)))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Trim()))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.Trim()))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Trim()))
            .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language.Trim()))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Genre, opt => opt.Ignore());

        CreateMap<Book, BookResponseDto>()
            .ForMember(dest => dest.BookType, opt => opt.MapFrom(src => src.BookType.ToString()))
            .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Genre.Name));
    }
}