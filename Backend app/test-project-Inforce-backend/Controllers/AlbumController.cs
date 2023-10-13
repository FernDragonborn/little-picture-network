using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using test_project_Inforce_backend.Data;
using test_project_Inforce_backend.Identity;
using test_project_Inforce_backend.Models;

namespace test_project_Inforce_backend.Controllers
{
    [ApiController]
    [Route("api/album")]
    public class AlbumController : Controller
    {
        private readonly TestProjectDbContext _context;

        public AlbumController(TestProjectDbContext context)
        {
            // TODO DbContext is not thread-safe so I'm not sure to inject it like this, need to check
            _context = context;
        }

        /// <summary>
        /// Gets array of albums of user
        /// </summary>
        /// <param name="id">Guid of user in string.</param>
        /// <returns>HTTP responce.</returns>
        /// <response code="200">Succesfully founded.</response>
        /// <response code="404">Photo doesn't exist.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("getUsersAlbums/{id}")]
        public IActionResult GetAllAlbumsOfUser(
            [FromRoute(Name = "id")] string id)
        {
            var guid = Guid.Parse(id);
            var albumsResponse = _context.Albums.Where(x => x.User.UserId == guid);
            if (albumsResponse.IsNullOrEmpty()) { return NotFound(); }

            return Ok(albumsResponse);
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateAlbum(
            [FromBody] AlbumDto albumDto)
        {
            Album album = new()
            {
                AlbumId = Guid.NewGuid()
            };
            Guid guid = Guid.Parse(albumDto.UserId);
            User user = _context.Users.FirstOrDefault(x => x.UserId == guid);
            if (user is null) { return BadRequest("Usr with this id doesn't exist"); }
            album.User = user;
            if (!albumDto.PhotoIds.IsNullOrEmpty())
            {
                album.Photos = new List<Photo>();
                foreach (var id in albumDto.PhotoIds)
                {
                    Guid photoGuid = Guid.Parse(id);
                    Photo? photo = _context.Photos.FirstOrDefault(x => x.PhotoId == photoGuid);

                    if (photo is null) { continue; }
                    album.Photos.Add(photo);
                }
            }

            try
            {
                _context.Albums.Add(album);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { return BadRequest(ex); }


            return Ok(album);
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
        public async Task<IActionResult> DeleteAnyalbum(
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
        /// Deletes photo by Id
        /// </summary>
        /// <param name="photoDto">The entity to remove.</param>
        /// <returns>HTTP responce.</returns>
        /// <response code="200">Succesfully deleted photo.</response>
        /// <response code="400">There could be mistakes in request or no such photo exists.</response>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAlbum(
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
