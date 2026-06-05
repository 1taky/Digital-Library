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

        CreateMap<BookFile, BookFileResponseDto>()
            .ForMember(
            destination => destination.FileCategory,
            options => options.MapFrom(source => source.FileCategory.ToString()));

        CreateMap<BookFormatRequestDto, BookFormat>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.BookId, opt => opt.Ignore())
            .ForMember(dest => dest.Book, opt => opt.Ignore())
            .ForMember(dest => dest.FormatType, opt => opt.MapFrom(src => Enum.Parse<BookFormatType>(src.FormatType, true)))
            .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => true));

        CreateMap<BookFormat, BookFormatResponseDto>()
            .ForMember(dest => dest.FormatType, opt => opt.MapFrom(src => src.FormatType.ToString()));

        CreateMap<CreateBookRequestDto, Book>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Trim()))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.Trim()))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Trim()))
            .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language.Trim()))
            .ForMember(dest => dest.GenreId, opt => opt.Ignore())
            .ForMember(dest => dest.Genre, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Files, opt => opt.Ignore())
            .ForMember(dest => dest.Formats, opt => opt.MapFrom(src => src.Formats));

        CreateMap<UpdateBookRequestDto, Book>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Trim()))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.Trim()))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Trim()))
            .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language.Trim()))
            .ForMember(dest => dest.GenreId, opt => opt.Ignore())
            .ForMember(dest => dest.Genre, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Files, opt => opt.Ignore())
            .ForMember(dest => dest.Formats, opt => opt.MapFrom(src => src.Formats));

        CreateMap<Book, BookResponseDto>()
            .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Genre.Name))
            .ForMember(dest => dest.HasCover, opt => opt.MapFrom(src =>
                src.Files.Any(file => file.FileCategory == FileCategory.Cover)))
            .ForMember(dest => dest.HasDownloadFile, opt => opt.MapFrom(src =>
                src.Files.Any(file => file.FileCategory == FileCategory.EBook)))
            .ForMember(dest => dest.HasAudioFile, opt => opt.MapFrom(src =>
                src.Files.Any(file => file.FileCategory == FileCategory.Audio)));

        CreateMap<Order, OrderResponseDto>()
            .ForMember(
                destination => destination.BookTitle,
                options => options.MapFrom(source => source.Book.Title))
            .ForMember(
                destination => destination.UserFullName,
                options => options.MapFrom(source => source.User.FullName))
            .ForMember(
                destination => destination.UserEmail,
                options => options.MapFrom(source => source.User.Email))
            .ForMember(
                destination => destination.Status,
                options => options.MapFrom(source => source.Status.ToString()))
            .ForMember(
                destination => destination.ManagerFullName,
                options => options.MapFrom(source =>
                    source.Manager == null ? null : source.Manager.FullName));
    }
}