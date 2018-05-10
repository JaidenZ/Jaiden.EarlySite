namespace EarlySite.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Reflection;
    using System.Web.Routing;
    using EarlySite.Core.DDD.Service;
    using EarlySite.Core.Component;
    using Jaiden.Redis;
    


    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //预备
            Prepared();

            
        }


        protected void Prepared()
        {
            //获取业务服务程序集
            Assembly businessdll = Assembly.Load("EarlySite.Business");
            //注册业务服务程序集
            ServiceObjectContainer.Load(businessdll);


            Thread work = Thread.CurrentThread;
            lock (work)
            {
                if(WorkThreadDictionary.Get(work) == null)
                {
                    WorkThreadDictionary.Create(Thread.CurrentThread);
                    EarlySite.Cache.Session.Deployment();
                }
            }

            //加载Redis
            //RedisClientManager.ReadOnlyHosts = new string[] { "192.168.11.103:6379" };
            //RedisClientManager.ReadOnlyHosts = new string[] { "192.168.11.103:6379" };



            //EarlySite.Cache.Session.Current.Set("test", "1111");

        }

    }
}
