using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EarlySite.Core.Utils;
using System.Threading;
using System.IO;
using System.Drawing;
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
            Image image = new Bitmap(path);




            Console.ReadKey(false);
        }

    }

}


