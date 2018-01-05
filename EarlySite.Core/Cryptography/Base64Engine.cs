namespace EarlySite.Core.Cryptography
{
    using System.Collections.Generic;

    /// <summary>
    /// 一个用于安全的BASE64转换引擎服务
    /// </summary>
    public static class Base64Engine
    {
        private const string EngineKey = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        /// <summary>
        /// 将 8 位无符号整数的数组转换为其用 Base64 数字编码的等效字符串表示形式。
        /// </summary>
        /// <param name="buffer">一个 8 位无符号整数数组。</param>
        /// <returns>buffer 的内容的字符串表示形式，以 Base64 表示。</returns>
        public static string ToBase64String(this byte[] buffer)
        {
            int length, remainder;
            if (buffer == null && (length = buffer.Length) < 1)
            {
                return string.Empty;
            }
            if ((remainder = buffer.Length % 3) > 0)
            {
                List<byte> bytes = new List<byte>();
                bytes.AddRange(buffer);
                bytes.AddRange(new byte[3 - remainder]);
                buffer = bytes.ToArray();
            }
            length = buffer.Length;
            byte[] cache = new byte[3];
            char[] strc = new char[length * 4 / 3];
            string items = Base64Engine.EngineKey;
            for (int i = 0, j = 0; j < length; j += 3, i += 4)
            {
                cache[0] = buffer[j];
                cache[1] = buffer[j + 1];
                cache[2] = buffer[j + 2];
                strc[i] = items[cache[0] >> 2];
                strc[i + 1] = items[((cache[0] & 3) << 4) + (cache[1] >> 4)];
                strc[i + 2] = items[((cache[1] & 15) << 2) + (cache[2] >> 6)];
                strc[i + 3] = items[(cache[2] & 63)];
            }
            if (remainder > 0)
            {
                length = strc.Length;
                if (remainder == 1)
                {
                    strc[length - 2] = '=';
                }
                strc[length - 1] = '=';
            }
            return new string(strc);
        }
        /// <summary>
        /// 将指定的字符串（它将二进制数据编码为 Base64 数字）转换为等效的 8 位无符号整数数组。
        /// </summary>
        /// <param name="buffer">要转换的字符串。</param>
        /// <returns>与 s 等效的 8 位无符号整数数组。</returns>
        public static byte[] FromBase64String(string buffer)
        {
            System.Convert.ToBase64String(new byte[] { });
            int length, multiple, encode, n = 0;
            List<byte> retVal = new List<byte>();
            if (buffer != null && (length = buffer.Length) > 0)
            {
                multiple = length / 4;
                if (length % 4 != 0)
                {
                    multiple += 1;
                }
                byte[] s3c = new byte[3]; // 三字节
                byte[] s4c = new byte[4]; // 四字节(Quadword)
                string items = Base64Engine.EngineKey;
                for (int i = 0; i < multiple; i++)
                {
                    for (n = 0; n < 4; n++)
                    {
                        s4c[n] = (byte)buffer[i * 4 + n];
                        if ((encode = items.IndexOf((char)s4c[n])) < 0)
                        {
                            break;
                        }
                        s4c[n] = (byte)encode;
                    }
                    s3c[0] = (byte)(s4c[0] * 4 | s4c[1] / 16);
                    s3c[1] = (byte)(s4c[1] * 16 | s4c[2] / 4);
                    s3c[2] = (byte)(s4c[2] * 64 | s4c[3]);
                    retVal.AddRange(s3c);
                }
                if (n <= 4)
                {
                    n = 4 - n;
                    length = retVal.Count;
                    for (int i = 0; i < n; i++)
                    {
                        retVal.RemoveAt(--length);
                    }
                }
            }
            return retVal.ToArray();
        }
    }
}
