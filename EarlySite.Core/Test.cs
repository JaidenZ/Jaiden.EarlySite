using System;
using EarlySite.Core.Cryptography;
namespace EarlySite.Core
{
    class Test
    {
        public static void Main(string[] args)
        {
            //for (int i = 0; i < 10; i++)
            //{
            //    Thread.Sleep(1000);
            //    Console.WriteLine(VerificationUtils.GetVefication());
            //}

            ////
            string path = @"D://TestCode//Jaiden.EarlySite//EarlySite.Web//Themes//Images//profileback.jpg";

            string base64 = Base64Image.GetBase64FromImage(path);


            Console.ReadKey(false);
        }

    }

}


