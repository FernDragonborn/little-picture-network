using test_project_Inforce_backend.Models;

namespace test_project_Inforce_backend.Data.Photo_Repository;
public interface IAlbumRepository : IDisposable
{
    void AddAlbum(Album album);
    List<Album> GetAllAlbums();
    Album? GetAlbumById(Guid id);
    void UpdateAlbum(Album album);
    void UpdateTable(List<Album> albums, Album album);
    void DeleteAlbum(Guid id);
    void Save();
}

