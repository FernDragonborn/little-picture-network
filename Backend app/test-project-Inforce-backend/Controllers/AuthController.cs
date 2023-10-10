using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Created("api/auth", user);
        }

        [Authorize]
        [HttpPost]
        public IActionResult authTest()
        {
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(UserDto userDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Login == userDto.Login);
            if (user is null) { return BadRequest("Wrong login or password"); }

            if (!BCrypt.Net.BCrypt.Verify(userDto.Password + user.Salt, user.PasswordHash))
            {
                return BadRequest("Wrong login or password");
            }

            string token = CreateToken(user);
            return Ok(token);
        }

        private string? CreateToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim("id", user.UserId.ToString()),
                new Claim("login", user.Login),
                new Claim("role", user.Role)
            };

            //TODO this needed to be provided by some key vault, but I didn't use these services, so it need enhentment. I also didn't use standalrt local vault, because it only can be acessed through builder for app
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("fNBXCRyN0a1CbXK6IVQwjnwUq6P3dF0DK2hbmvm"));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtObj = new JwtSecurityToken(
                issuer: "https://localhost:7245",
                audience: "https://localhost:7245",
                notBefore: DateTime.UtcNow.AddMinutes(-2),
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(5)),
                signingCredentials: credentials);
            var jwt = new JwtSecurityTokenHandler().WriteToken(jwtObj);

            return jwt;
        }
    }
}
