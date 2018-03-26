namespace EarlySite.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Reflection;
    using System.Web.Routing;
    using EarlySite.Core.DDD.Service;
    


    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //注册业务服务
            ServiceObjectContainer.Load(Assembly.LoadFrom("bin/EarlySite.Business.dll"));













            //获取当前执行程序集,以获取路径,得到业务程序集进行加载
            //Assembly assembly = Assembly.GetExecutingAssembly();

        }
    }
}
