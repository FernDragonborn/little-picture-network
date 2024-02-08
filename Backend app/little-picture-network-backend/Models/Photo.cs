using LittlePictureNetworkBackend.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace LittlePictureNetworkBackend.Models;
public class Photo
{
    public Photo() { }

    public Photo(PhotoDto photoDto)
    {
        if (photoDto.PhotoId.IsNullOrEmpty())
        {
            PhotoId = Guid.NewGuid();
        }
        else if (photoDto.PhotoId == "00000000-0000-0000-0000-000000000000")
        {
            PhotoId = Guid.NewGuid();
        }
        else
        {
            PhotoId = Guid.Parse(photoDto.PhotoId);
        }
        LikesCount = photoDto.LikesCount;
        DislikesCount = photoDto.DislikesCount;
    }

    public Photo(uint likesCount, uint disLikesCount)
    {
        PhotoId = Guid.NewGuid();
        LikesCount = likesCount;
        DislikesCount = disLikesCount;
    }

    public Photo(byte[] photoData)
    {
        PhotoData = photoData;
        LikesCount = 0;
        DislikesCount = 0;
    }

    [Key]
    internal Guid PhotoId { get; set; }

    [MaxLength(100)]
    internal string? Name { get; set; }

    internal byte[]? PhotoData { get; set; }

    internal byte[]? PrewievData { get; set; }

    [Required]
    internal uint? LikesCount { get; set; }

    [Required]
    internal uint? DislikesCount { get; set; }
    //TODO rewrite with List<Like>
    internal virtual EFGuidCollection? LikesList { get; set; }

    internal virtual EFGuidCollection? DisikesList { get; set; }

    [Timestamp]
    private byte[] Version { get; set; }
}
