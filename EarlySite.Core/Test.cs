using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EarlySite.Core.Utils;
using System.Threading;
namespace EarlySite.Core
{
    class Test
    {
        public static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine(VerificationUtils.GetVefication());
            }

            //


            Console.ReadKey(false);
        }

    }

}


