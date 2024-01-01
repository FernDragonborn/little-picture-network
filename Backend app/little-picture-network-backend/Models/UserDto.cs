namespace LittlePictureNetworkBackend.Models;
public class UserDto
{
    public UserDto() { }

    public UserDto(User user)
    {
        UserId = user.UserId.ToString();
        Role = user.Role;
        Name = user.Name;
        Login = user.Login;
    }

    public string? UserId { get; set; }

    public string? Role { get; set; }

    public string? Name { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public string? JwtToken { get; set; }
}