using LittlePictureNetworkBackend.Models;

namespace LittlePictureNetworkBackend.DTOs;
public class PhotoDto
{
    public PhotoDto() { }

    public PhotoDto(string? photoId, string? name, string? photoData, string? previewData, uint? likesCount,
        uint? dislikesCount)
    {
        PhotoId = photoId;
        Name = name;
        PhotoData = photoData;
        PrewievData = previewData;
        LikesCount = likesCount;
        DislikesCount = dislikesCount;
    }

    public PhotoDto(Photo photo)
    {
        PhotoId = photo.PhotoId.ToString();
        //UserId = photo.User.UserId.ToString();
        Name = photo.Name;
        PhotoData = photo.PhotoData.ToString();
        PrewievData = photo.PrewievData.ToString();
        LikesCount = photo.LikesCount;
        DislikesCount = photo.DislikesCount;
    }

    public string? PhotoId { get; set; }

    public string? AlbumId { get; set; }

    public string? Name { get; set; }

    public string? PhotoData { get; set; }

    public string? PrewievData { get; set; }

    public uint? LikesCount { get; set; }

    public uint? DislikesCount { get; set; }
}