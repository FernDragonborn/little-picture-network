namespace test_project_Inforce_backend.Models;
public class PhotoDto
{
    public PhotoDto() { }

    public PhotoDto(string? photoId, string? userId, string? name, string? photoData, string? previewData, uint? likesCount,
        uint? dislikesCount)
    {
        PhotoId = photoId;
        UserId = userId;
        Name = name;
        PhotoData = photoData;
        PrewievData = previewData;
        LikesCount = likesCount;
        DislikesCount = dislikesCount;
    }

    public string? PhotoId { get; set; }

    public string? UserId { get; set; }

    public string? Name { get; set; }

    public string? PhotoData { get; set; }

    public string? PrewievData { get; set; }

    public uint? LikesCount { get; set; }

    public uint? DislikesCount { get; set; }
}