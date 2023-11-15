using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using test_project_Inforce_backend.Data;
using test_project_Inforce_backend.Data.Album_Repository;
using test_project_Inforce_backend.Data.Photo_Repository;
using test_project_Inforce_backend.Identity;
using test_project_Inforce_backend.Interfaces;
using test_project_Inforce_backend.Models;

namespace test_project_Inforce_backend.Controllers
{
    [ApiController]
    [Route("api/album")]
    public class AlbumController : Controller
    {
        private readonly IPhotoConverter _photoConverter;
        private IAlbumRepository _albumRepository;

        public AlbumController(IPhotoConverter photoConverter)
        {
            _photoConverter = photoConverter;
            _albumRepository = new AlbumRepository(ContextFactory.CreateNew());
        }

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
                //TODO add check for null (?)
                Photo photo = context.Photos.FirstOrDefault(x => x.PhotoId == photoDto.PhotoId);
                photoResponse.Add(new PhotoDto(photo));
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
        [HttpGet("getUsersAlbums/{id}")]
        public IActionResult GetAllAlbumsOfUser(
            [FromRoute(Name = "id")] string id)
        {
            using var context = ContextFactory.CreateNew();
            var guid = Guid.Parse(id);
            var albumsResponse = context.Albums.Where(x => x.User.UserId == guid).ToArray();
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

        [Authorize]
        [HttpPost("addPhotoToAlbum")]
        public async Task<IActionResult> AddPhotoToAlbum(
            [FromBody] PhotoDto photoDto)
        {
            if (photoDto.AlbumId.IsNullOrEmpty())
            {
                return BadRequest("Album id was null or empty");
            }

            if (photoDto.UserId.IsNullOrEmpty())
            {
                return BadRequest("User id was null or empty");
            }

            using var context = ContextFactory.CreateNew();
            Photo photoResponse = new(photoDto);
            photoResponse.PhotoData = _photoConverter.ToByteArray(photoDto.PhotoData);

            var albumGuid = Guid.Parse(photoDto.AlbumId);
            var userGuid = Guid.Parse(photoDto.UserId);


            //var album = context.Albums.FirstOrDefault(x => x.AlbumId == albumGuid && x.User.UserId == userGuid);
            var album = _albumRepository.GetAlbumById(albumGuid);
            album.User = new User()
            {
                UserId = Guid.Parse(photoDto.UserId)
            };

            if (album.Photos is null)
            {
                album.Photos = new List<Photo>();
            }

            album.Photos.Add(photoResponse);

            try
            {
                //context.Update(album);
                //context.Entry(album.User).State = EntityState.Modified;
                //foreach (var relatedEntity in album.Photos)
                //{
                //    context.Entry(relatedEntity).State = EntityState.Modified;
                //}
                ////context.Albums.Entry(album).State = EntityState.Modified;
                //await context.SaveChangesAsync();
                _albumRepository.UpdateAlbum(album);
                _albumRepository.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.Clear();
                Console.WriteLine(ex + "\n--------------\n" + ex.StackTrace);
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok(photoDto);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAlbum(
            [FromBody] AlbumDto albumDto)
        {
            await using var context = ContextFactory.CreateNew();
            Album album = new()
            {
                AlbumId = Guid.NewGuid(),
                Title = albumDto.Title,
                User = new User() { UserId = Guid.Parse(albumDto.UserId) }
            };
            Guid guid = Guid.Parse(albumDto.UserId);
            User user = context.Users.FirstOrDefault(x => x.UserId == guid);
            if (user is null) { return BadRequest("Usr with this id doesn't exist"); }
            album.User = user;
            if (!albumDto.PhotoIds.IsNullOrEmpty())
            {
                album.Photos = new List<Photo>();
                foreach (var id in albumDto.PhotoIds)
                {
                    Guid photoGuid = Guid.Parse(id);
                    Photo? photo = context.Photos.FirstOrDefault(x => x.PhotoId == photoGuid);

                    if (photo is null) { continue; }
                    album.Photos.Add(photo);
                }
            }

            try
            {
                context.Albums.Add(album);
                await context.SaveChangesAsync();
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
