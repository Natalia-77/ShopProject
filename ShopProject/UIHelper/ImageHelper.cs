using System;
using System.Drawing;
using System.IO;


namespace ShopProject.UIHelper
{
    public static class ImageHelper
    {
        public static string ImageConvertToBase64(string path)
        {
            using Image image = Image.FromFile(path);
            using MemoryStream m = new MemoryStream();
            image.Save(m, image.RawFormat);
            byte[] imageBytes = m.ToArray();
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }
        public static Bitmap LoadBase64(this string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            using var ms = new MemoryStream(bytes, 0, bytes.Length);
            Image image = Image.FromStream(ms, true);
            return new Bitmap(image);          

        }
    }
}
