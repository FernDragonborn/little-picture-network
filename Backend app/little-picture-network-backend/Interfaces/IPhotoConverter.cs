namespace LittlePictureNetworkBackend.Interfaces;

public interface IPhotoConverter
{
    byte[] ToJpeg(byte[] photoData);
    byte[] ToByteArray(string photoDataStr);
}
