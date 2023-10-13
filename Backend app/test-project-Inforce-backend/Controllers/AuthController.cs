using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test_project_Inforce_backend.Data;
using test_project_Inforce_backend.Models;


namespace test_project_Inforce_backend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly TestProjectDbContext _context;

        public AuthController(TestProjectDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param></param>
        /// <param name="userDto">user data transfer object. Neded fields for this method: login, password</param>
        /// <returns>HTTP responce.</returns>
        /// <response code="201">Succesfully regitered</response>
        /// <response code="400">User with this login already exists or Database exception</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            if (_context.Users.FirstOrDefault(x => x.Login == userDto.Login) != default)
            {
                return BadRequest("User with this login already exists");
            }

            User user = new();
            user.Salt = BCrypt.Net.BCrypt.GenerateSalt();
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password + user.Salt, workFactor: 13);

            user.Login = userDto.Login;
            user.Role = userDto.Role;

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { return BadRequest(ex); }

            string token = JwtHandler.CreateToken(user);
            userDto = new UserDto(user);
            userDto.JwtToken = token;
            return Created("api/auth", userDto);
        }

        /// <summary>
        /// Log in and get JWT token
        /// </summary>
        /// <param></param>
        /// <param name="userDto">user data transfer object. Neded fields for this method: login, password</param>
        /// <returns>HTTP responce and JWT token in it.</returns>
        /// <response code="200">Succesfully logged in</response>
        /// <response code="400">Wrong login or password</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("login")]
        public async Task<IActionResult> LogIn(UserDto userDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Login == userDto.Login);
            if (user is null) { return BadRequest("Wrong login or password"); }

            if (!BCrypt.Net.BCrypt.Verify(userDto.Password + user.Salt, user.PasswordHash))
            {
                return BadRequest("Wrong login or password");
            }

            string token = JwtHandler.CreateToken(user);
            userDto = new UserDto(user);
            userDto.JwtToken = token;
            return Ok(userDto);
        }

        /// <summary>
        /// Method for testing if authoraization works
        /// </summary>
        /// <param></param>
        /// <param name="userDto">user data transfer object. Neded fields for this method: login, password</param>
        /// <returns>HTTP responce.</returns>
        /// <response code="200">Succesfully authorized</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpPost("renewToken")]
        public async Task<IActionResult> RenewToken(UserDto userDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Login == userDto.Login);
            if (user is null) { return BadRequest("User not found"); }
            string token = JwtHandler.CreateToken(user);
            userDto = new UserDto(user);
            userDto.JwtToken = token;
            return Ok(userDto);
        }
    }
}
