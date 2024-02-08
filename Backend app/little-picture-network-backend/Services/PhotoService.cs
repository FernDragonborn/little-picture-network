using LittlePictureNetworkBackend.Data;
using LittlePictureNetworkBackend.DTOs;
using LittlePictureNetworkBackend.Interfaces;
using LittlePictureNetworkBackend.Models;
using Microsoft.IdentityModel.Tokens;

namespace LittlePictureNetworkBackend.Services;
public class PhotoService : IDisposable
{
    private readonly PictureNetworkDbContext _context;
    private readonly IVirusScanner _virusScanner;
    private readonly IPhotoConverter _photoConverter;

    public PhotoService(PictureNetworkDbContext context, IVirusScanner virusScanner, IPhotoConverter photoConverter)
    {
        _context = context;
        _virusScanner = virusScanner;
        _photoConverter = photoConverter;
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
        lock (_context)
        {
            _context.Photos.Entry(originalPhoto).CurrentValues.SetValues(photoRequest);
            _context.SaveChanges();
        }
        return photoRequest;
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

