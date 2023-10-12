using System.Drawing;
using System.Drawing.Imaging;
using test_project_Inforce_backend.Interfaces;

namespace test_project_Inforce_backend
{
    public class SimplePhotoConverter : IPhotoConverter
    {

        public byte[] ConvertToJpeg(byte[] photoData)
        {
            var ms = new MemoryStream(photoData);
            string path = @"D:\data\temp.jpeg";
            FileStream fs = new(path, FileMode.Create, FileAccess.ReadWrite);
            fs.Write(photoData);
            fs.Close();
            byte[] resposePhotoData = null;
            using (Bitmap bmp = new(path))
            {
                bmp.Save(ms, ImageFormat.Jpeg);

                ImageConverter converter = new ImageConverter();
                resposePhotoData = (byte[])converter.ConvertTo(bmp, typeof(byte[]));
            }

            //TODO I'm not sure, maybe I can use await here for optimization
            ms.Dispose();
            fs.Dispose();

            File.WriteAllBytes(path, resposePhotoData);


            return resposePhotoData;
        }
    }
}
