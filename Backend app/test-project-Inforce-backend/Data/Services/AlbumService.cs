using Microsoft.EntityFrameworkCore;
using test_project_Inforce_backend.Models;

namespace test_project_Inforce_backend.Data.Services;
public class AlbumService
{
    private TestProjectDbContext _context;

    public AlbumService(TestProjectDbContext context)
    {
        _context = context;
    }

    public List<Album> GetAllAlbumsOfUser(User user)
    {
        var albumList = _context.Albums.Where(x => x.User.UserId == user.UserId);
        return albumList.ToList();
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
        lock (_context)
        {
            _context.Albums.Add(album);
            _context.SaveChanges();
        }
    }

    public void UpdateAlbum(Album album)
    {
        lock (_context)
        {
            _context.Entry(album).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }

    public void DeleteAlbum(Guid Id)
    {
        Album album = _context.Albums.Find(Id);
        if (album is null) { throw new ArgumentNullException("Id not found in table"); }
        _context.Albums.Remove(album);
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
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

