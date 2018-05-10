namespace EarlySite.Core.Cryptography
{
    using System.IO;
    using System.Drawing;
    using System;

    public static class Base64Image
    {
        /// <summary>
        /// 根据base64字符串返回GDI+位图
        /// </summary>
        /// <param name="base64str"></param>
        /// <returns></returns>
        public static Bitmap GetImageFromBase64(string base64str)
        {
            byte[] data = Convert.FromBase64String(base64str);
            MemoryStream ms = new MemoryStream(data);
            Bitmap bitmap = new Bitmap(ms);
            return bitmap;
        }


        /// <summary>
        /// 获取图片的base64字符串
        /// </summary>
        /// <param name="imagefile"></param>
        /// <returns></returns>
        public static string GetBase64FromImage(string imagefile)
        {
            if (!File.Exists(imagefile))
            {
                return "";
            }

            Bitmap bmp = new Bitmap(imagefile);
            return GetBase64FromImage(bmp);
        }

        /// <summary>
        /// 获取图片的base64字符串
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static string GetBase64FromImage(Image image)
        {
            string base64str = "";
            try
            {
                using(MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] data = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(data, 0, (int)ms.Length);
                    ms.Close();

                    base64str = Convert.ToBase64String(data);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return base64str;
        }

    }
}
