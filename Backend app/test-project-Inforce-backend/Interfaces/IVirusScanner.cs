namespace test_project_Inforce_backend.Interfaces
{
    public interface IVirusScanner
    {
        bool ScanPhotoForViruses(byte[] photoData);
    }
}
