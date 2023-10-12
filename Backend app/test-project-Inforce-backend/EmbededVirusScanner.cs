using AntiVirus;
using test_project_Inforce_backend.Interfaces;

namespace test_project_Inforce_backend
{
    public class EmbededVirusScanner : IVirusScanner
    {
        AntiVirus.Scanner scanner = new AntiVirus.Scanner();
        public bool ScanPhotoForViruses(byte[] photoData)
        {
            const string path = "tempForScanning";
            File.WriteAllBytes(path, photoData);
            var result = scanner.ScanAndClean(path);
            return result.HasFlag(ScanResult.VirusNotFound);
        }
    }
}
