using LittlePictureNetworkBackend.DTOs;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LittlePictureNetworkBackend.Models;

public class Album
{
    public Album() { }

    public Album(AlbumDto albumDto)
    {
        AlbumId = Guid.Parse(albumDto.AlbumId);
        Title = albumDto.Title;
    }

    [Key]
    internal Guid AlbumId { get; set; }

    [Required]
    [ForeignKey("UserId")]
    internal virtual User User { get; set; }

    internal virtual List<Photo>? Photos { get; set; }

    [Required]
    [MaxLength(100)]
    internal string? Title { get; set; }

    [Timestamp]
    private byte[] Version { get; set; }
}