namespace test_project_Inforce_backend.Models;
public class UserDto
{
    public Guid? UserId { get; set; }

    public string Role { get; set; }

    public string? Name { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }
}