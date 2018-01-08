namespace EarlySite.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Cache;
    using EarlySite.Business.Constract;
    using EarlySite.Business.IService;

    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Account = base.CurrentAccount;



            return View();
        }

        public ActionResult Test()
        {
            IAccount service = new AccountService();
            service.SendRegistEmail(null);
            return View();
        }

    }
}