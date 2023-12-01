using Microsoft.EntityFrameworkCore;
using test_project_Inforce_backend.Models;

namespace test_project_Inforce_backend.Data.Album_Repository;
public class AlbumRepository
{
    private TestProjectDbContext _context;

    public AlbumRepository(TestProjectDbContext context)
    {
        this._context = context;
    }

    public List<Album> GetAllAlbums()
    {
        var a = _context.Albums;
        return a.ToList();
    }

    public Album? GetAlbumById(Guid Id)
    {
        return _context.Albums.Find(Id);
    }

    public void AddAlbum(AlbumDto albumDto)
    {
        Album album = new(albumDto)
        {
            User = _context.Users.Find(Guid.Parse(albumDto.UserId)) ??
                   throw new ArgumentException("user with this id does not exist"),
            AlbumId = Guid.NewGuid(),
        };
        _context.Albums.Add(album);
    }

    public void UpdateAlbum(Album album)
    {
        _context.Entry(album).State = EntityState.Modified;
    }

    public void UpdateTable(List<Album> albums, Album album)
    {
        _context.Entry(album).State = EntityState.Modified;
        _context.Albums.UpdateRange(albums);
    }

    public void DeleteAlbum(Guid Id)
    {
        Album album = _context.Albums.Find(Id);
        if (album is null) { throw new ArgumentNullException("Id not found in table"); }
        _context.Albums.Remove(album);
    }

    public void Save()
    {
        lock (_context)
        {
            _context.SaveChanges();
        }
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
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

