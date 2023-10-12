namespace test_project_Inforce_backend.Interfaces
{
    public interface IPhotoScaler
    {
        byte[] ConvertToJpeg(byte[] photoData);
    }
}
