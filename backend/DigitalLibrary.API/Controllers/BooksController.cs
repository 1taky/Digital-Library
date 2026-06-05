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
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly IMapper _mapper;

    public BooksController(
        IBookService bookService,
        IMapper mapper)
    {
        _bookService = bookService;
        _mapper = mapper;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<BookResponseModel>>> GetAll(
    [FromQuery] BookFilterRequestModel filterModel)
    {
        BookFilterRequestDto requestDto =
            _mapper.Map<BookFilterRequestDto>(filterModel);

        List<BookResponseDto> responseDto =
            await _bookService.GetFilteredAsync(requestDto);

        List<BookResponseModel> responseModel =
            _mapper.Map<List<BookResponseModel>>(responseDto);

        foreach (BookResponseModel model in responseModel)
        {
            BookResponseDto dto = responseDto.First(book => book.Id == model.Id);

            FillFileUrls(model, dto);
        }

        return Ok(responseModel);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<BookResponseModel>> GetById(int id)
    {
        BookResponseDto responseDto = await _bookService.GetByIdAsync(id);

        BookResponseModel responseModel =
            _mapper.Map<BookResponseModel>(responseDto);

        FillFileUrls(responseModel, responseDto);

        return Ok(responseModel);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<BookResponseModel>> Create(
        CreateBookRequestModel model)
    {
        CreateBookRequestDto requestDto =
            _mapper.Map<CreateBookRequestDto>(model);

        BookResponseDto responseDto =
            await _bookService.CreateAsync(requestDto);

        BookResponseModel responseModel =
            _mapper.Map<BookResponseModel>(responseDto);

        FillFileUrls(responseModel, responseDto);

        return Ok(responseModel);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<BookResponseModel>> Update(
        int id,
        UpdateBookRequestModel model)
    {
        UpdateBookRequestDto requestDto =
            _mapper.Map<UpdateBookRequestDto>(model);

        BookResponseDto responseDto =
            await _bookService.UpdateAsync(id, requestDto);

        BookResponseModel responseModel =
            _mapper.Map<BookResponseModel>(responseDto);

        FillFileUrls(responseModel, responseDto);

        return Ok(responseModel);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _bookService.DeleteAsync(id);

        return NoContent();
    }

    private void FillFileUrls(BookResponseModel model, BookResponseDto dto)
    {
        if (dto.HasCover)
        {
            model.CoverUrl = Url.Action(
                action: "GetCover",
                controller: "BookFiles",
                values: new { bookId = dto.Id },
                protocol: Request.Scheme);
        }

        if (dto.HasDownloadFile)
        {
            model.DownloadUrl = Url.Action(
                action: "Download",
                controller: "BookFiles",
                values: new { bookId = dto.Id },
                protocol: Request.Scheme);
        }

        if (dto.HasAudioFile)
        {
            model.ListenUrl = Url.Action(
                action: "Listen",
                controller: "BookFiles",
                values: new { bookId = dto.Id },
                protocol: Request.Scheme);
        }
    }
}