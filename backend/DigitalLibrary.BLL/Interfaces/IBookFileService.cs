using DigitalLibrary.BLL.DTOs.Responses;

namespace DigitalLibrary.BLL.Interfaces;

public interface IBookFileService
{
    Task<BookFileResponseDto> UploadCoverAsync(
        int bookId,
        string fileName,
        string contentType,
        long fileSize,
        Stream fileStream);

    Task<BookFileResponseDto> UploadEBookAsync(
        int bookId,
        string fileName,
        string contentType,
        long fileSize,
        Stream fileStream);

    Task<BookFileResponseDto> UploadAudioAsync(
        int bookId,
        string fileName,
        string contentType,
        long fileSize,
        Stream fileStream);

    Task<BookFileResponseDto> GetCoverAsync(int bookId);

    Task<BookFileResponseDto> GetEBookAsync(int bookId);

    Task<BookFileResponseDto> GetAudioAsync(int bookId);
}