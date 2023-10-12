using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using test_project_Inforce_backend.Data;
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
        public PhotoController(TestProjectDbContext context, IVirusScanner virusScanner)
        {
            //DbContext is not thread-safe< so I;
            _context = context;
            _virusScanner = virusScanner;
        }


        /// <summary>
        /// Gets stringArr book by it's id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>HTTP responce.</returns>
        /// <response code="200">Succesfully founded</response>
        /// <response code="404">Book doesn't exist</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetPhotoById(
            [FromRoute] Guid id)
        {
            var photoResponse = await _context.Photos.FirstOrDefaultAsync(x => x.PhotoId == id);
            if (photoResponse is null) { return NotFound(); }
            return Ok(photoResponse);
        }

        /// <summary>
        /// Gets stringArr book by it's id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>HTTP responce.</returns>
        /// <response code="200">Succesfully founded</response>
        /// <response code="404">Book doesn't exist</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("getAny")]
        public async Task<IActionResult> GetAllPhotos()
        {
            var photos = _context.Photos.ToList();
            if (photos is null || photos.Count == 0) { return NotFound(); }


            List<PhotoDto> photoResponse = new();
            try
            {
                photos.ForEach(x => photoResponse.Add(
                    new PhotoDto()
                    {
                        PhotoId = x.PhotoId.ToString(),
                        PhotoData = x.PhotoData.ToString(),
                        LikesCount = x.LikesCount,
                        DislikesCount = x.DislikesCount
                    }
                ));
                string s = Encoding.ASCII.GetString(photos[0].PhotoData);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }

            return Ok(photos);
        }

        /// <summary>
        /// Gets array of books which names contains stringArr request string
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
        //    var photos = _context.Photos.Where(x => x.Title.Contains(title));
        //    if (photos.Count() is 0) { return NotFound("No book with such name"); }
        //    return Ok(photos);
        //}

        /// <summary>
        /// Add new book
        /// </summary>
        /// <param name="photoRequest"></param>
        /// <returns></returns>
        /// <response code="201">Succesfully added photo</response>
        /// <response code="404">Book doesn't exist</response>

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("add")]
        public async Task<IActionResult> AddPhoto(
            [FromBody] PhotoDto photoRequest)
        {
            var stringArr = photoRequest.PhotoData.Split(',');
            Photo photo = new();
            photo.LikesCount = 0;
            photo.DislikesCount = 0;
            //I know that this isn't right, but Convert.FromBase64String haven't warked. I'm in search how to fix this
            photo.PhotoData = stringArr.Select(str => Convert.ToByte(str)).ToArray();

            bool noViruses = _virusScanner.ScanPhotoForViruses(photo.PhotoData);
            if (!noViruses) { return BadRequest("image containing viruses"); }



            photo.User = _context.Users.FirstOrDefault(x => x.UserId.ToString() == photoRequest.UserId);
            if (photo.User is null)
            {
                /*return BadRequest("User was null"); */
                photo.User = _context.Users.FirstOrDefault(x => x.Login == "Fern");
            }

            try
            {
                await _context.Photos.AddAsync(photo);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Created("api/photo", photo);
        }

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
    //    var photos = _context.Photos.FirstOrDefault(x => x.PhotoId == photoRequest.PhotoId);
    //    if (photos is null) { return NotFound("Book with this Id doesn't exist"); }
    //    _context.Entry(photos).CurrentValues.SetValues(photoRequest);
    //    await _context.SaveChangesAsync();
    //    return Ok(photos);
    //}

    /// <summary>
    /// Deletes stringArr specific book
    /// </summary>
    /// <param name="photoRequest">The entity to remove.</param>
    /// <returns>HTTP responce.</returns>
    /// <response code="200">Succesfully deleted stringArr book.</response>
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

