namespace EarlySite.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Cache;
    using EarlySite.Business.Constract;
    using EarlySite.Business.IService;
    using EarlySite.Core.DDD.Service;

    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Account = base.CurrentAccount;

            


            return View();
        }

        public ActionResult Test()
        {
            ServiceObjectContainer.Get<IAccount>();
            return View();
        }

    }
}