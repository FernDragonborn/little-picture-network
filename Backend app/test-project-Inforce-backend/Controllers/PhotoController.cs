using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using test_project_Inforce_backend.Data;
using test_project_Inforce_backend.Identity;
using test_project_Inforce_backend.Interfaces;
using test_project_Inforce_backend.Models;

namespace test_project_Inforce_backend.Controllers
{
    [ApiController]
    [Route("api/photo")]
    public class PhotoController : Controller
    {
        private readonly TestProjectDbContext _context;
        private readonly IVirusScanner _virusScanner;
        private readonly IPhotoConverter _photoConverter;

        public PhotoController(TestProjectDbContext context, IVirusScanner virusScanner, IPhotoConverter photoConverter)
        {
            // TODO DbContext is not thread-safe so I'm not sure to inject it like this, need to check
            _context = context;
            _virusScanner = virusScanner;
            _photoConverter = photoConverter;
        }


        /// <summary>
        /// Gets photoRequest data transfer object by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>HTTP responce.</returns>
        /// <response code="200">Succesfully founded</response>
        /// <response code="404">Photo doesn't exist</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetPhotoById(
            [FromRoute] Guid id)
        {
            //TODO remake for photoDto, not just Photo
            var photoResponse = await _context.Photos.FirstOrDefaultAsync(x => x.PhotoId == id);
            if (photoResponse is null)
            {
                return NotFound();
            }

            return Ok(photoResponse);
        }


        //TODO delete ot else
        /// <summary>
        /// Gets all photos in photoRequest data transfer objects
        /// </summary>
        /// <returns>HTTP responce.</returns>
        /// <response code="200">Succesfully founded</response>
        /// <response code="400">Exception initializtion of object occured</response>
        /// <response code="404">Photo doesn't exist</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("getAllPhotos")]
        public IActionResult GetAllPhotos()
        {
            var photos = _context.Photos.ToList();
            if (photos.IsNullOrEmpty()) { return NotFound(); }

            List<PhotoDto> photoResponse = new();
            try
            {
                photos.ForEach(x => photoResponse.Add(new PhotoDto(x)));
            }
            catch (Exception ex) { return BadRequest(ex); }

            return Ok(photos);
        }


        /// <summary>
        /// Add new photoRequest.
        /// </summary>
        /// <param name="photoRequest">photoRequest data transfer object. Needed fields for this method: photoData, userId</param >
        /// <returns>HTTP responce.</returns>
        /// <response code="201">Succesfully added photoRequest.</response>
        /// <response code = "400">Image contained viruses, user isn't existing or database exception.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddPhoto(
            [FromBody] PhotoDto photoRequest)
        {
            Photo photo = new(0, 0);

            if (photoRequest.PhotoData.IsNullOrEmpty()) { return BadRequest("PhotoData was empty."); }

            photo.PhotoData = _photoConverter.ToByteArray(photoRequest.PhotoData);

            bool noViruses = _virusScanner.ScanPhotoForViruses(photo.PhotoData);
            if (!noViruses) { return BadRequest("Image contains viruses."); }

            photo.PhotoData = _photoConverter.ToJpeg(photo.PhotoData);

            //photo.User = _context.Users.FirstOrDefault(x => x.UserId.ToString() == photoRequest.UserId);
            //if (photo.User is null) { return BadRequest("User was null"); }

            try
            {
                await _context.Photos.AddAsync(photo);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { return BadRequest(ex); }

            return Created("api/photoDto", photo);
        }

        /// <summary>
        /// Edit an existing photoDto.
        /// </summary>
        /// <param name = "photoDto" ></param >
        /// <returns>HTTP responce.</returns >
        /// <response code="200">Succesfully edited photoDto.</response>
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
            Photo photoRequest = new Photo(photoDto);
            var guid = Guid.Parse(photoDto.PhotoId);
            var photoResponse = _context.Photos.FirstOrDefault(x => x.PhotoId == guid);
            if (photoResponse is null) { return NotFound("Photo with this Id doesn't exist"); }

            try
            {
                _context.Photos.Entry(photoRequest).CurrentValues.SetValues(photoDto);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { return BadRequest(ex); }

            return Ok(photoResponse);
        }

        /// <summary>
        /// Deletes photoDto by Id
        /// </summary>
        /// <param name="photoDto">The entity to remove.</param>
        /// <returns>HTTP responce.</returns>
        /// <response code="200">Succesfully deleted photoDto.</response>
        /// <response code="400">There could be mistakes in request or no such photoDto exists.</response>>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = IdentityData.AdminUserPolicyName)]
        [HttpDelete("deleteAny")]
        public async Task<IActionResult> DeleteAnyPhotoById(
            [FromBody] PhotoDto photoDto)
        {
            Photo photoRequest = new Photo(photoDto);
            try
            {
                _context.Photos.Remove(photoRequest);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { return BadRequest(ex); }

            return Ok();
        }

        /// <summary>
        /// Deletes photoDto by Id
        /// </summary>
        /// <param name="photoDto">The entity to remove.</param>
        /// <returns>HTTP responce.</returns>
        /// <response code="200">Succesfully deleted photoDto.</response>
        /// <response code="400">There could be mistakes in request or no such photoDto exists.</response>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeletePhotoById(
            [FromBody] PhotoDto photoDto)
        {
            Photo photoRequest = new Photo(photoDto);
            try
            {
                _context.Photos.Remove(photoRequest);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { return BadRequest(ex); }

            return Ok();
        }
    }
}

