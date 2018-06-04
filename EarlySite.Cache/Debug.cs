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

            Session.DeploymentForWeb();


            

            OnlineAccountInfo online = new OnlineAccountInfo();
            online.BackCorver = ConstInfo.DefaultBackCover;
            online.Phone = 11111111111;
            online.NickName = "test";
            online.Description = "1";

            

            Session.Current.Set("test",online);


            OnlineAccountInfo backcover = Session.Current.Get<OnlineAccountInfo>("test");

        }

    }


}
