using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EarlySite.Model.Common;
using EarlySite.Model.Database;
namespace EarlySite.Cache
{


    class Debug
    {
        public static void Main(string[] args)
        {


            sta_elements("12Mmnn2");

            Console.ReadKey(false);

            //Session.DeploymentForWeb();
            
            //OnlineAccountInfo online = new OnlineAccountInfo();
            //online.BackCorver = ConstInfo.DefaultBackCover;
            //online.Phone = 11111111111;
            //online.NickName = "test";
            //online.Description = "1";

            

            //Session.Current.Set("test",online);


            //OnlineAccountInfo backcover = Session.Current.Get<OnlineAccountInfo>("test");

        }




        public static void sta_elements(string input)
        {
            Dictionary<char, int> dic = new Dictionary<char, int>();

            for (int i = 0; i < input.Length; i++)
            {
                char temp = input[i];
                int count = 0;
                for (int t = 0; t < input.Length; t++)
                {
                    if (input[t] == temp)
                    {
                        count++;
                    }
                }

                if (!dic.ContainsKey(temp))
                {
                    dic.Add(temp, count);
                } 
            }

            foreach (KeyValuePair<char,int> pai in dic)
            {
                Console.WriteLine(string.Format("{0}-{1}",pai.Key,pai.Value));
            }

        }


    }


}
