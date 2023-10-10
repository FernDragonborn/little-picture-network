using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test_project_Inforce_backend.Models;
public class Photo
{
    [Key]
    public Guid PhotoId { get; set; }

    [Required]
    [ForeignKey("User")]
    public Guid AuthorId { get; set; }

    public virtual User User { get; set; }

    [MaxLength(100)]
    public string? Name { get; set; }

    [NotMapped]
    public byte[]? PhotoData { get; set; }

    [NotMapped]
    public byte[]? PrewievData { get; set; }

    [Required]
    public uint Likes { get; set; }

    [Required]
    public uint Dislikes { get; set; }
}
