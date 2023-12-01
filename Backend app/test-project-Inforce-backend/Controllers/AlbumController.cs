using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using test_project_Inforce_backend.Data;
using test_project_Inforce_backend.Data.Album_Repository;
using test_project_Inforce_backend.Data.Photo_Repository;
using test_project_Inforce_backend.Identity;
using test_project_Inforce_backend.Models;

namespace test_project_Inforce_backend.Controllers
{
    [ApiController]
    [Route("api/album")]
    public class AlbumController : Controller
    {
        private readonly AlbumRepository _albumRepository;
        private readonly IPhotoRepository _photoRepository;

        public AlbumController()
        {
            _photoRepository = new PhotoRepository(
                    ContextFactory.CreateNew(),
                    new WindowsEmbededVirusScanner(),
                    new SimplePhotoConverter()
                );
            _albumRepository = new AlbumRepository(
                ContextFactory.CreateNew()
                );
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("getAlbumPhotos/{id:Guid}")]
        public async Task<IActionResult> GetAlbumPhotos(
            [FromRoute] Guid id)
        {
            await using var context = ContextFactory.CreateNew();
            var album = context.Albums.FirstOrDefault(x => x.AlbumId == id);
            if (album is null) { return NotFound(); }
            //TODO needs optimization
            List<PhotoDto> photoResponse = new();

            if (album.Photos.IsNullOrEmpty()) { return NotFound("No photos added to album yet"); }

            foreach (var photoDto in album.Photos)
            {
                //TODO check and rewrite
                //TODO add check for null (?)
                //Photo photo = context.Photos.FirstOrDefault(x => x.PhotoId.ToString() == photoDto.PhotoId);
                //photoResponse.Add(new PhotoDto(photo));
            }

            return Ok(photoResponse);
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
        [HttpGet("getUsersAlbums/{id:Guid}")]
        public IActionResult GetAllAlbumsOfUser(
            [FromRoute(Name = "id")] Guid id)
        {
            using var context = ContextFactory.CreateNew();
            var albumsResponse = context.Albums.Where(x => x.User.UserId == id).ToArray();
            if (albumsResponse.IsNullOrEmpty()) { return NotFound(); }

            return Ok(albumsResponse);
        }

        /// <summary>
        /// Updates album by Id.
        /// </summary>
        /// <param name="albumDto">The entity to update.</param>
        /// <returns>HTTP responce.</returns>
        /// <response code="200">Succesfully deleted photo.</response>
        /// <response code="401">The id of user in album and id of user does'nt match.</response>
        /// <response code="404">There could be mistakes in request or no such photo exists.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateAlbum(
            [FromBody] AlbumDto albumDto)
        {
            await using var context = ContextFactory.CreateNew();
            Album albumResponse = new(albumDto);
            albumResponse = context.Albums.FirstOrDefault(x => x.AlbumId == albumResponse.AlbumId);
            if (albumResponse is null) { return NotFound("Album with this id doesn't exist"); }

            if (albumResponse.User.UserId.ToString() != albumDto.UserId) { return Unauthorized("The id of user in album and id of user does'nt match"); }

            try
            {
                context.Albums.Update(albumResponse);
                await context.SaveChangesAsync();
            }
            catch (Exception ex) { return BadRequest(ex); }

            return Ok(albumResponse);
        }

        /// <summary>
        /// Adds id of photo to album collection
        /// </summary>
        /// <param name="photoDto"></param>
        /// <returns>HTTP responce</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpPost("addPhotoToAlbum")]
        public async Task<IActionResult> AddPhotoToAlbum(
            [FromBody] PhotoDto photoDto)
        {
            #region check for null and empty
            if (photoDto.AlbumId.IsNullOrEmpty())
            {
                return BadRequest("Album id was null or empty");
            }
            if (photoDto.UserId.IsNullOrEmpty())
            {
                return BadRequest("User id was null or empty");
            }
            if (photoDto.PhotoId.IsNullOrEmpty())
            {
                return BadRequest("User id was null or empty");
            }
            if (photoDto.PhotoData.IsNullOrEmpty())
            {
                return BadRequest("Photo data was null or empty");
            }
            #endregion

            var albumGuid = Guid.Parse(photoDto.AlbumId);
            var album = _albumRepository.GetAlbumById(albumGuid);
            if (album is null) return BadRequest("album with this id does not exist");

            Photo photoResponse;
            try
            {
                photoResponse = await _photoRepository.AddPhotoAsync(photoDto);
                _photoRepository.Save();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            album.Photos ??= new();
            album.Photos.Add(photoResponse);

            _albumRepository.UpdateAlbum(album);
            _albumRepository.Save();

            return Ok(album);
        }


        /// <summary>
        /// Creates new album in database
        /// </summary>
        /// <param name="albumDto"></param>
        /// <returns>HTTP responce</returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAlbum(
            [FromBody] AlbumDto albumDto)
        {
            #region check for null and empty
            if (albumDto.UserId.IsNullOrEmpty())
            {
                return BadRequest("User id was null or empty");
            }
            if (albumDto.Title.IsNullOrEmpty())
            {
                return BadRequest("Album id was null or empty");
            }
            #endregion

            try
            {
                _albumRepository.AddAlbum(albumDto);
                _albumRepository.Save();
            }
            catch (ArgumentException ex)
            {
                if (ex.InnerException is null) return BadRequest(ex.Message);
                else return BadRequest(ex.InnerException.Message);
            }

            return Ok(albumDto);
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
        public async Task<IActionResult> DeleteAnyAlbum(
            [FromBody] PhotoDto photoDto)
        {
            await using var context = ContextFactory.CreateNew();
            Photo photoRequest = new Photo(photoDto);
            try
            {
                context.Photos.Remove(photoRequest);
                await context.SaveChangesAsync();
            }
            catch (Exception ex) { return BadRequest(ex); }

            return Ok();
        }

        /// <summary>
        /// Deletes album by Id
        /// </summary>
        /// <param name="albumDto">The entity to remove.</param>
        /// <returns>HTTP responce.</returns>
        /// <response code="200">Succesfully deleted photo.</response>
        /// <response code="401">The id of user in album and id of user does'nt match.</response>
        /// <response code="404">There could be mistakes in request or no such photo exists.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAlbum(
            [FromBody] AlbumDto albumDto)
        {
            await using var context = ContextFactory.CreateNew();
            Album albumResponse = new(albumDto);
            albumResponse = context.Albums.FirstOrDefault(x => x.AlbumId == albumResponse.AlbumId);
            if (albumResponse is null) { return NotFound("Album with this id doesn't exist"); }

            if (albumResponse.User.UserId.ToString() != albumDto.UserId) { return Unauthorized("The id of user in album and id of user does'nt match"); }

            try
            {
                context.Albums.Remove(albumResponse);
                await context.SaveChangesAsync();
            }
            catch (Exception ex) { return BadRequest(ex); }

            return Ok(albumResponse);
        }
    }
}
