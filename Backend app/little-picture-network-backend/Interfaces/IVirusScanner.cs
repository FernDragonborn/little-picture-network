namespace LittlePictureNetworkBackend.Interfaces;

public interface IVirusScanner
{
    bool ScanPhotoForViruses(byte[] photoData);
}
