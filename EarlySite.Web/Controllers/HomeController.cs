namespace EarlySite.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Cache;

    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Account = base.CurrentAccount;



            return View();
        }

        public ActionResult Test()
        {
            throw new Exception("Test ");
            return View();
        }

    }
}