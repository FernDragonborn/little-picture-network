using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using test_project_Inforce_backend.Interfaces;
using test_project_Inforce_backend.Models;

namespace test_project_Inforce_backend.Data.Services;
public class PhotoService
{
    private readonly TestProjectDbContext _context;
    private readonly IVirusScanner _virusScanner;
    private readonly IPhotoConverter _photoConverter;

    public PhotoService(TestProjectDbContext context, IVirusScanner virusScanner, IPhotoConverter photoConverter)
    {
        _context = context;
        _virusScanner = virusScanner;
        _photoConverter = photoConverter;
    }

    public Photo? GetPhotoById(Guid Id)
    {
        return _context.Photos.Find(Id);
    }

    public PhotoDto? GetPhotoDtoById(Guid Id)
    {
        return new PhotoDto(_context.Photos.Find(Id));
    }

    public async Task<Photo> AddPhotoAsync(PhotoDto photoDto)
    {
        if (photoDto.PhotoData.IsNullOrEmpty()) { throw new ArgumentException("PhotoData was empty."); }

        Photo photo = new(0, 0)
        {
            //User = await _context.Users.FindAsync(Guid.Parse(photoDto.UserId)),
            PhotoData = _photoConverter.ToByteArray(photoDto.PhotoData)
        };

        bool noViruses = _virusScanner.ScanPhotoForViruses(photo.PhotoData);
        if (!noViruses) { throw new ArgumentException("Image contains viruses."); }

        photo.PhotoData = _photoConverter.ToJpeg(photo.PhotoData);

        lock (_context)
        {
            _context.Photos.Add(photo);
            _context.SaveChanges();
        }
        return photo;
    }

    public PhotoDto UpdatePhoto(PhotoDto photoRequest)
    {
        var originalPhoto = _context.Photos.Find(photoRequest.PhotoId);
        _context.Photos.Entry(originalPhoto).CurrentValues.SetValues(photoRequest);
        return photoRequest;
    }

    public void UpdateTable(List<Photo> photos, Photo photo)
    {
        _context.Entry(photo).State = EntityState.Modified;
        _context.Photos.UpdateRange(photos);
    }

    public void DeletePhoto(Guid Id)
    {
        var photo = _context.Photos.Find(Id) ?? throw new ArgumentNullException("Id not found in table");
        lock (_context)
        {
            _context.Photos.Remove(photo);
            _context.SaveChanges();
        }
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
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

