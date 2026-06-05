using AutoMapper;
using DigitalLibrary.API.Models.Responses;
using DigitalLibrary.BLL.DTOs.Responses;
using DigitalLibrary.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalLibrary.API.Controllers;

[ApiController]
[Route("api/books/{bookId:int}")]
public class BookFilesController : ControllerBase
{
    private readonly IBookFileService _bookFileService;
    private readonly IMapper _mapper;

    public BookFilesController(
        IBookFileService bookFileService,
        IMapper mapper)
    {
        _bookFileService = bookFileService;
        _mapper = mapper;
    }

    [HttpPost("cover")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<BookFileResponseModel>> UploadCover(
        int bookId,
        IFormFile file)
    {
        await using Stream stream = file.OpenReadStream();

        BookFileResponseDto responseDto =
            await _bookFileService.UploadCoverAsync(
                bookId,
                file.FileName,
                file.ContentType,
                file.Length,
                stream);

        BookFileResponseModel responseModel =
            _mapper.Map<BookFileResponseModel>(responseDto);

        return Ok(responseModel);
    }

    [HttpPost("file")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<BookFileResponseModel>> UploadEBook(
        int bookId,
        IFormFile file)
    {
        await using Stream stream = file.OpenReadStream();

        BookFileResponseDto responseDto =
            await _bookFileService.UploadEBookAsync(
                bookId,
                file.FileName,
                file.ContentType,
                file.Length,
                stream);

        BookFileResponseModel responseModel =
            _mapper.Map<BookFileResponseModel>(responseDto);

        return Ok(responseModel);
    }

    [HttpPost("audio")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<BookFileResponseModel>> UploadAudio(
        int bookId,
        IFormFile file)
    {
        await using Stream stream = file.OpenReadStream();

        BookFileResponseDto responseDto =
            await _bookFileService.UploadAudioAsync(
                bookId,
                file.FileName,
                file.ContentType,
                file.Length,
                stream);

        BookFileResponseModel responseModel =
            _mapper.Map<BookFileResponseModel>(responseDto);

        return Ok(responseModel);
    }

    [HttpGet("cover")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCover(int bookId)
    {
        BookFileResponseDto responseDto =
            await _bookFileService.GetCoverAsync(bookId);

        if (!System.IO.File.Exists(responseDto.FilePath))
        {
            return NotFound();
        }

        Stream stream = System.IO.File.OpenRead(responseDto.FilePath);

        return File(
            stream,
            responseDto.ContentType,
            responseDto.FileName);
    }

    [HttpGet("download")]
    [Authorize]
    public async Task<IActionResult> Download(int bookId)
    {
        BookFileResponseDto responseDto =
            await _bookFileService.GetEBookAsync(bookId);

        if (!System.IO.File.Exists(responseDto.FilePath))
        {
            return NotFound();
        }

        Stream stream = System.IO.File.OpenRead(responseDto.FilePath);

        return File(
            stream,
            responseDto.ContentType,
            responseDto.FileName);
    }

    [HttpGet("listen")]
    [Authorize]
    public async Task<IActionResult> Listen(int bookId)
    {
        BookFileResponseDto responseDto =
            await _bookFileService.GetAudioAsync(bookId);

        if (!System.IO.File.Exists(responseDto.FilePath))
        {
            return NotFound();
        }

        Stream stream = System.IO.File.OpenRead(responseDto.FilePath);

        return File(
            stream,
            responseDto.ContentType,
            enableRangeProcessing: true);
    }
}