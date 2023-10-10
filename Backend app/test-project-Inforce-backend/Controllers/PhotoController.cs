using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using test_project_Inforce_backend.Data;
using test_project_Inforce_backend.Models;

namespace test_project_Inforce_backend.Controllers
{
    [ApiController]
    [Route("api/photo")]
    public class PhotoController : Controller
    {
        private readonly TestProjectDbContext _context;

        public PhotoController(TestProjectDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets photos in db
        /// </summary>
        /// <returns>HTTP responce.</returns>
        /// <response code="200">Succesfully founded</response>
        /// <response code="404">Database is empty</response>
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[HttpGet]
        //public async Task<IActionResult> GetSeveralAlbums()
        //{
        //    var books = await _context.Photos.ToListAsync();
        //    if (books.Count == 0) { return NotFound(); }
        //    return Ok(books);
        //}

        /// <summary>
        /// Gets a book by it's id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>HTTP responce.</returns>
        /// <response code="200">Succesfully founded</response>
        /// <response code="404">Book doesn't exist</response>
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[HttpGet("{id:guid}")]
        //public async Task<IActionResult> GetPhotoById(
        //    [FromRoute] Guid id)
        //{
        //    var bookResponse = await _context.Photos.FirstOrDefaultAsync(x => x.PhotoId == id);
        //    if (bookResponse is null) { return NotFound(); }
        //    return Ok(bookResponse);
        //}

        /// <summary>
        /// Gets array of books which names contains a request string
        /// </summary>
        /// <param name="title">fd</param>
        /// <returns>HTTP responce.</returns>
        /// <response code="200">Succesfully founded.</response>
        /// <response code="404">Book doesn't exist.</response>
        //[HttpGet("{title}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public IActionResult GetBooksByName(
        //    [FromRoute(Name = "title")] string title)
        //{
        //    var bookResponse = _context.Photos.Where(x => x.Title.Contains(title));
        //    if (bookResponse.Count() is 0) { return NotFound("No book with such name"); }
        //    return Ok(bookResponse);
        //}

        /// <summary>
        /// Add new book
        /// </summary>
        /// <param name="photoRequest"></param>
        /// <returns></returns>
        /// <response code="201">Succesfully added photo</response>
        /// <response code="404">Book doesn't exist</response>
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("add")]
        public async Task<IActionResult> AddPhoto(
            [FromBody] Photo photoRequest)
        {
            Photo photo = new()
            {
                PhotoData = photoRequest.PhotoData,
                User = photoRequest.User
            };
            await _context.Photos.AddAsync(photo);
            await _context.SaveChangesAsync();
            return Created("api/photo", photo);
        }

        /// <summary>
        /// Edit an existing book
        /// </summary>
        /// <param name="photoRequest"></param>
        /// <returns></returns>
        /// <response code="200">Succesfully edited boook.</response>
        /// <response code="404">If book doesn't exist.</response>
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[HttpPut("edit")]
        //public async Task<IActionResult> EditBook(
        //    [FromBody] Photo photoRequest)
        //{
        //    var bookResponse = _context.Photos.FirstOrDefault(x => x.PhotoId == photoRequest.PhotoId);
        //    if (bookResponse is null) { return NotFound("Book with this Id doesn't exist"); }
        //    _context.Entry(bookResponse).CurrentValues.SetValues(photoRequest);
        //    await _context.SaveChangesAsync();
        //    return Ok(bookResponse);
        //}

        /// <summary>
        /// Deletes a specific book
        /// </summary>
        /// <param name="photoRequest">The entity to remove.</param>
        /// <returns>HTTP responce.</returns>
        /// <response code="200">Succesfully deleted a book.</response>
        /// <response code="400">Tere could be mistakes in request, no such book.</response>
        //[HttpDelete("delete")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> DeleteBookById(
        //    [FromBody] Photo photoRequest)
        //{
        //    try
        //    {
        //        var deletedBook = _context.Photos.Remove(photoRequest).Entity;
        //        await _context.SaveChangesAsync();
        //        return Ok(deletedBook);
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        return BadRequest("There could be mistakes in request, no such book");
        //    }
        //}
    }
}
