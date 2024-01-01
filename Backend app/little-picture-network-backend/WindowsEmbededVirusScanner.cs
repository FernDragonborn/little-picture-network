using AntiVirus;
using LittlePictureNetworkBackend.Interfaces;

namespace LittlePictureNetworkBackend;

public class WindowsEmbededVirusScanner : IVirusScanner
{
    private AntiVirus.Scanner scanner = new AntiVirus.Scanner();
    public bool ScanPhotoForViruses(byte[] photoData)
    {
        //TODO rewrite
        const string path = @"D:\data\temp.txt";
        File.WriteAllBytes(path, photoData);
        var result = scanner.ScanAndClean(path);
        return result.HasFlag(ScanResult.VirusNotFound);
    }
}
