using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test_project_Inforce_backend.Models;

public class Album
{
    public Album() { }

    public Album(AlbumDto albumDto)
    {
        AlbumId = Guid.Parse(albumDto.AlbumId);
        Title = albumDto.Title;
    }

    [Key]
    public Guid AlbumId { get; set; }

    [Required]
    [ForeignKey("UserId")]
    public virtual User User { get; set; }

    public virtual List<Photo> Photos { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Title { get; set; }

    [Timestamp]
    public byte[] Version { get; set; }
}