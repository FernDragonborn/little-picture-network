using test_project_Inforce_backend.Models;

namespace test_project_Inforce_backend.Data.Photo_Repository;
public interface IPhotoRepository : IDisposable
{
    Task<PhotoDto> AddPhotoAsync(PhotoDto photoDto);
    Photo? GetPhotoById(Guid id);
    PhotoDto? GetPhotoDtoById(Guid id);
    PhotoDto? UpdatePhoto(PhotoDto photoDto);
    void DeletePhoto(Guid id);
    void Save();
}

