namespace LittlePictureNetworkBackend.Interfaces;

public interface IPhotoScaler
{
    byte[] ConvertToJpeg(byte[] photoData);
}
