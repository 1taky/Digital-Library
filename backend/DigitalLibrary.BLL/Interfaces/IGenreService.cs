using DigitalLibrary.BLL.DTOs.Requests;
using DigitalLibrary.BLL.DTOs.Responses;

namespace DigitalLibrary.BLL.Interfaces;

public interface IGenreService
{
    Task<List<GenreResponseDto>> GetAllAsync();

    Task<GenreResponseDto> CreateAsync(CreateGenreRequestDto request);

    Task DeleteAsync(int id);
}