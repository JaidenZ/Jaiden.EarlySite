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

            //获取业务服务程序集
            Assembly businessdll = Assembly.Load("EarlySite.Business");
            //注册业务服务程序集
            ServiceObjectContainer.Load(businessdll);
            
        }
    }
}
