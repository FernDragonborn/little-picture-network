using LittlePictureNetworkBackend.Interfaces;
using System.Drawing;
using System.Drawing.Imaging;

namespace LittlePictureNetworkBackend;

public class SimplePhotoConverter : IPhotoConverter
{

    public byte[] ToByteArray(string photoDataStr)
    {
        //I know that this isn't right, but Convert.FromBase64String haven't warked. I'm in search how to fix this
        var stringArr = photoDataStr.Split(',');
        byte[] photoDataByte = stringArr.Select(str => Convert.ToByte(str))
            .ToArray();
        return photoDataByte;
    }

    public byte[] ToJpeg(byte[] photoData)
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

        return resposePhotoData;
    }
}
