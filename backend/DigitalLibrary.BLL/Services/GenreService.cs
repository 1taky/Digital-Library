using AutoMapper;
using DigitalLibrary.BLL.DTOs.Requests;
using DigitalLibrary.BLL.DTOs.Responses;
using DigitalLibrary.BLL.Exceptions;
using DigitalLibrary.BLL.Interfaces;
using DigitalLibrary.DAL.Entities;
using DigitalLibrary.DAL.Interfaces;

namespace DigitalLibrary.BLL.Services;

public class GenreService : IGenreService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GenreService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<GenreResponseDto>> GetAllAsync()
    {
        List<Genre> genres = await _unitOfWork.Genres.GetAllAsync();

        return _mapper.Map<List<GenreResponseDto>>(genres);
    }

    public async Task<GenreResponseDto> CreateAsync(CreateGenreRequestDto request)
    {
        ValidateCreateGenreRequest(request);

        bool genreExists = await _unitOfWork.Genres.ExistsByNameAsync(request.Name);

        if (genreExists)
        {
            throw new BadRequestException("Жанр з такою назвою вже існує.");
        }

        Genre genre = _mapper.Map<Genre>(request);

        await _unitOfWork.Genres.AddAsync(genre);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<GenreResponseDto>(genre);
    }

    public async Task DeleteAsync(int id)
    {
        Genre? genre = await _unitOfWork.Genres.GetByIdAsync(id);

        if (genre == null)
        {
            throw new NotFoundException("Жанр не знайдено.");
        }

        _unitOfWork.Genres.Delete(genre);

        await _unitOfWork.SaveChangesAsync();
    }

    private static void ValidateCreateGenreRequest(CreateGenreRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new BadRequestException("Назва жанру є обов'язковою.");
        }

        if (request.Name.Trim().Length < 2)
        {
            throw new BadRequestException("Назва жанру має містити мінімум 2 символи.");
        }
    }
}