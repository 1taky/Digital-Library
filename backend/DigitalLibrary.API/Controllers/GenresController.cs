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
[Route("api/genres")]
public class GenresController : ControllerBase
{
    private readonly IGenreService _genreService;
    private readonly IMapper _mapper;

    public GenresController(
        IGenreService genreService,
        IMapper mapper)
    {
        _genreService = genreService;
        _mapper = mapper;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<GenreResponseModel>>> GetAll()
    {
        List<GenreResponseDto> responseDto = await _genreService.GetAllAsync();

        List<GenreResponseModel> responseModel =
            _mapper.Map<List<GenreResponseModel>>(responseDto);

        return Ok(responseModel);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GenreResponseModel>> Create(
        CreateGenreRequestModel model)
    {
        CreateGenreRequestDto requestDto =
            _mapper.Map<CreateGenreRequestDto>(model);

        GenreResponseDto responseDto =
            await _genreService.CreateAsync(requestDto);

        GenreResponseModel responseModel =
            _mapper.Map<GenreResponseModel>(responseDto);

        return Ok(responseModel);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _genreService.DeleteAsync(id);

        return NoContent();
    }
}