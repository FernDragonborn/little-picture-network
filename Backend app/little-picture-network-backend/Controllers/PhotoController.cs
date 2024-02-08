using LittlePictureNetworkBackend.Data;
using LittlePictureNetworkBackend.DTOs;
using LittlePictureNetworkBackend.Identity;
using LittlePictureNetworkBackend.Models;
using LittlePictureNetworkBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LittlePictureNetworkBackend.Controllers;

[ApiController]
[Route("api/photo")]
public class PhotoController : Controller
{
    private readonly PhotoService _photoService;
    private readonly PictureNetworkDbContext _context;

    public PhotoController()
    {
        _photoService = new PhotoService(
            ContextFactory.CreateNew(),
            new WindowsEmbededVirusScanner(),
            new SimplePhotoConverter()
            );
        _context = ContextFactory.CreateNew();
    }


    /// <summary>
    /// Gets photoDto by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>HTTP responce.</returns>
    /// <response code="200">Succesfully founded</response>
    /// <response code="404">Photo doesn't exist</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetPhotoById(
        [FromRoute] Guid id)
    {
        Photo? photoResponse = _context.Photos.Find(id);
        if (photoResponse is null)
        {
            return NotFound();
        }

        return Ok(new PhotoDto(photoResponse));
    }


    /// <summary>
    /// Add new photoRequest.
    /// </summary>
    /// <param name="photoDto">photo data transfer object. Needed fields for this method: photo, userId</param >
    /// <returns>HTTP responce.</returns>
    /// <response code="201">Succesfully added photo.</response>
    /// <response code = "400">Image contained viruses, user isn't existing or database exception.</response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]
    [HttpPost("add")]
    public async Task<IActionResult> AddPhoto(
        [FromBody] PhotoDto photoDto)
    {
        Photo photoResponse;
        try
        {
            photoResponse = await _photoService.AddPhotoAsync(photoDto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        return Created("api/photoDto", new PhotoDto(photoResponse));
    }

    /// <summary>
    /// Edit an existing photo.
    /// </summary>
    /// <param name = "photoDto" ></param >
    /// <returns>HTTP responce.</returns >
    /// <response code="200">Succesfully edited photo.</response>
    /// <response code="400">Database exception.</response>
    /// <response code="404">Photo does not exist.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    [HttpPut("edit")]
    public async Task<IActionResult> EditPhoto(
        [FromBody] PhotoDto photoDto)
    {
        var photoResponse = _photoService.UpdatePhoto(photoDto);
        if (photoResponse is null)
        {
            throw new ArgumentException("Photo with this id does not exist");
        }

        return Ok(photoResponse);
    }

    /// <summary>
    /// Deletes photo by Id
    /// </summary>
    /// <param name="photoDto">The entity to remove.</param>
    /// <returns>HTTP responce.</returns>
    /// <response code="200">Succesfully deleted photo.</response>
    /// <response code="400">There could be mistakes in request or no such photo exists.</response>>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpDelete("deleteAny")]
    public async Task<IActionResult> DeleteAnyPhotoById(
        [FromBody] PhotoDto photoDto)
    {
        if (photoDto.PhotoId is null) return BadRequest("Id was null");
        try
        {
            var photo = _context.Photos.Find(Guid.Parse(photoDto.PhotoId)) ?? throw new ArgumentNullException("Id not found in table");
            lock (_context)
            {
                _context.Photos.Remove(photo);
                _context.SaveChanges();
            }
        }
        catch (Exception ex) { return BadRequest(ex); }

        return Ok("succesfully deleted");
    }

    /// <summary>
    /// Deletes photo by Id
    /// </summary>
    /// <param name="photoDto">The entity to remove.</param>
    /// <returns>HTTP responce.</returns>
    /// <response code="200">Succesfully deleted photo.</response>
    /// <response code="400">There could be mistakes in request or no such photo exists.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]
    [HttpDelete("delete")]
    public async Task<IActionResult> DeletePhotoById(
        [FromBody] PhotoDto photoDto)
    {
        if (photoDto.PhotoId is null) return BadRequest("Id was null");
        try
        {
            var photo = _context.Photos.Find(Guid.Parse(photoDto.PhotoId)) ?? throw new ArgumentNullException("Id not found in table");
            lock (_context)
            {
                _context.Photos.Remove(photo);
                _context.SaveChanges();
            }
        }
        catch (Exception ex) { return BadRequest(ex); }

        return Ok("succesfully deleted");
    }
}
