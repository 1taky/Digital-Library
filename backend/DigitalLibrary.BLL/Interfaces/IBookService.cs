using DigitalLibrary.BLL.DTOs.Requests;
using DigitalLibrary.BLL.DTOs.Responses;

namespace DigitalLibrary.BLL.Interfaces;

public interface IBookService
{
    Task<List<BookResponseDto>> GetAllAsync();

    Task<List<BookResponseDto>> GetFilteredAsync(BookFilterRequestDto request);

    Task<BookResponseDto> GetByIdAsync(int id);

    Task<BookResponseDto> CreateAsync(CreateBookRequestDto request);

    Task<BookResponseDto> UpdateAsync(int id, UpdateBookRequestDto request);

    Task DeleteAsync(int id);
}