namespace test_project_Inforce_backend.Interfaces
{
    public interface IPhotoConverter
    {
        byte[] ToJpeg(byte[] photoData);
        byte[] ToByteArray(string photoDataStr);
    }
}
