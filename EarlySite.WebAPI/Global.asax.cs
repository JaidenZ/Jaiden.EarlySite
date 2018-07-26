using EarlySite.Core.Component;
using EarlySite.Core.DDD.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace EarlySite.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //预备
            Prepared();
        }



        protected void Prepared()
        {
            //获取业务服务程序集
            Assembly businessdll = Assembly.Load("EarlySite.Business");
            //获取业务服务程序集
            Assembly cachedll = Assembly.Load("EarlySite.Cache");
            //注册业务服务程序集
            ServiceObjectContainer.Load(businessdll);
            ServiceObjectContainer.Load(cachedll);

            Thread work = Thread.CurrentThread;
            lock (work)
            {
                if (WorkThreadDictionary.Get(work) == null)
                {
                    WorkThreadDictionary.Create(Thread.CurrentThread);
                    //EarlySite.Cache.Session.Deployment();
                    EarlySite.Cache.Session.DeploymentForWeb();
                }
            }

            //加载Redis
            //RedisClientManager.ReadOnlyHosts = new string[] { "192.168.11.103:6379" };
            //RedisClientManager.ReadOnlyHosts = new string[] { "192.168.11.103:6379" };



            //EarlySite.Cache.Session.Current.Set("test", "1111");

        }
    }
}
