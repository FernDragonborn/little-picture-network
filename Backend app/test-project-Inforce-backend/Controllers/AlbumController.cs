using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using test_project_Inforce_backend.Data;

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
    }
}
