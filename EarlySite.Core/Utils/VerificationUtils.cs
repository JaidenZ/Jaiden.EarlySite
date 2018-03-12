namespace EarlySite.Core.Utils
{
    using System;

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


        private static string GetCode()
        {
            int millisecond = DateTime.Now.Millisecond;
            int seed = millisecond % 26;
            string code = _CODELIST[seed].ToString();

            return code;
        }
        
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
