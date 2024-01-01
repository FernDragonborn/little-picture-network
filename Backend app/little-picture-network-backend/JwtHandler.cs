using LittlePictureNetworkBackend.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LittlePictureNetworkBackend;

public static class JwtHandler
{
    public static string? CreateToken(User user)
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
            notBefore: DateTime.UtcNow.AddMinutes(-1),
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(20)),
            signingCredentials: credentials);
        var jwt = new JwtSecurityTokenHandler().WriteToken(jwtObj);

        return jwt;
    }
}