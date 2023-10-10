using System.ComponentModel.DataAnnotations;

namespace test_project_Inforce_backend.Models;
public class User
{
    [Key]
    public Guid UserId { get; set; }

    [Required]
    public string Role { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    [MaxLength(50)]
    public string Login { get; set; }

    [Required]
    [MaxLength(50)]
    public string PasswordHash { get; set; }

    [Required]
    [MaxLength(25)]
    public string Salt { get; set; }
}