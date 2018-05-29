using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EarlySite.Model.Database;
namespace EarlySite.Cache
{


    class Debug
    {
        public static void Main(string[] args)
        {

            Session.DeploymentForWeb();


            IList<string> list  = Session.Current.ScanAllKeys("OnlineAI_*");



            OnlineAccountInfo online = Session.Current.Get<OnlineAccountInfo>(list[0]);



        }

    }


}
