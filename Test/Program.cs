using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using EarlySite.Core.Cryptography;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {


            FileInfo file = new FileInfo(@"D:\\TestCode\\Jaiden.EarlySite\\EarlySite.Web\\Themes\\Images");
            FileStream stream = new FileStream(@"D:\\defaultHead.png", FileMode.Open);
            byte[] headdata = new byte[stream.Length];
            if (stream.CanRead)
            {
                int redabyte = 0;
                do
                {
                    redabyte = stream.Read(headdata, 0, headdata.Length);

                }
                while (redabyte > 0);
            }
            stream.Close();

            string base64 = Base64Engine.ToBase64String(headdata);


        }
    }
}
