using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace test_project_Inforce_backend.Models;
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
    public Guid PhotoId { get; set; }

    //[Required]
    //[ForeignKey("UserId")]
    //public virtual User User { get; set; }

    [MaxLength(100)]
    public string? Name { get; set; }

    public byte[]? PhotoData { get; set; }

    public byte[]? PrewievData { get; set; }

    [Required]
    public uint? LikesCount { get; set; }

    [Required]
    public uint? DislikesCount { get; set; }

    public virtual EFGuidCollection? LikesList { get; set; }

    public virtual EFGuidCollection? DisikesList { get; set; }

    [Timestamp]
    public byte[] Version { get; set; }
}
