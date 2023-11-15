using Microsoft.EntityFrameworkCore;
using test_project_Inforce_backend.Data.Photo_Repository;
using test_project_Inforce_backend.Models;

namespace test_project_Inforce_backend.Data.Album_Repository;
public class AlbumRepository : IAlbumRepository
{
    private TestProjectDbContext _context;

    public AlbumRepository(TestProjectDbContext context)
    {
        this._context = context;
    }

    public IEnumerable<Album> GetAllAlbums()
    {
        return _context.Albums;
    }

    public Album? GetAlbumById(Guid Id)
    {
        return _context.Albums.Find(Id);
    }

    public void AddAlbum(Album album)
    {
        _context.Albums.Add(album);
    }

    public void UpdateAlbumViaList(List<Album> album)
    {
        _context.Entry(album).State = EntityState.Modified;
    }

    public void UpdateAlbum(Album album)
    {
        _context.Entry(album).State = EntityState.Modified;
    }

    public void UpdateTable(List<Album> albums)
    {
        throw new NotImplementedException();
        //_context.Albums = albums;
    }

    public void DeleteAlbum(Guid Id)
    {
        Album album = _context.Albums.Find(Id);
        if (album is null) { throw new ArgumentNullException("Id not found in table"); }
        _context.Albums.Remove(album);
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

