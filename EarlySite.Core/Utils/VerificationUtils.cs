namespace EarlySite.Core.Utils
{
    using System;
    using System.IO;
    using System.Drawing;
    using System.Drawing.Imaging;

    /// <summary>
    /// 验证码工具类
    /// </summary>
    public class VerificationUtils
    {

        private static char[] _CODELIST = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        private static int[] _NUMLIST = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        public static string GetVefication()
        {
            string verificationcode = "";
            for (int i = 0; i < 5; i++)
            {
                int millisecond = DateTime.Now.Millisecond;

                if(i == 0)
                {
                    verificationcode += GetNum();
                }
                else
                {
                    if (millisecond % i > 0)
                    {
                        verificationcode += GetNum();
                    }
                    else
                    {
                        verificationcode += GetCode();
                    }
                }
            }
            return verificationcode;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="length">验证码长度</param>
        /// <returns></returns>
        public static string GetVefication(int length)
        {
            string verificationcode = "";
            for (int i = 0; i < length; i++)
            {
                int millisecond = DateTime.Now.Millisecond;

                if (i == 0)
                {
                    verificationcode += GetNum();
                }
                else
                {
                    if (millisecond % i > 0)
                    {
                        verificationcode += GetNum();
                    }
                    else
                    {
                        verificationcode += GetCode();
                    }
                }
            }
            return verificationcode;
        }

        /// <summary>
        /// 获取图形验证码位图
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Bitmap GetVeficationImage(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            Random rand = new Random();

            Bitmap veficationmap = new Bitmap(100, 38);
            Graphics g = Graphics.FromImage(veficationmap);
            g.FillRectangle(Brushes.White, new Rectangle(0, 0, veficationmap.Width, veficationmap.Height));
            for (int i = 0; i < value.Length; i++)
            {
                int x = i*20 +  rand.Next(20);
                int y = 3 +  rand.Next(5);
                g.DrawString(value.Substring(i,1), new Font("Microsoft YaHei", rand.Next(12,18), rand.Next() > 20? FontStyle.Bold:FontStyle.Italic), Brushes.RoyalBlue, x, y);
            }
            return veficationmap;
        }

        /// <summary>
        /// 获图形取验证码字节
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] GetVeficationByte(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            Bitmap map = GetVeficationImage(value);
            MemoryStream ms = new MemoryStream();
            map.Save(ms, ImageFormat.Bmp);
            byte[] result = ms.GetBuffer();
            return result;
        }

        /// <summary>
        /// 获取验证码字符
        /// </summary>
        /// <returns></returns>
        private static string GetCode()
        {
            int millisecond = DateTime.Now.Millisecond;
            int seed = millisecond % 26;
            string code = _CODELIST[seed].ToString();

            return code;
        }
        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <returns></returns>
        private static string GetNum()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            int result = (BitConverter.ToInt32(bytes, 0) % 10);
            
            return Math.Abs(result).ToString();
        }
        
    }
}
