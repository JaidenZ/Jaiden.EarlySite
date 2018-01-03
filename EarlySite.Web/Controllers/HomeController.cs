namespace EarlySite.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using EarlySite.SModel;

    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            Account account = new Account();
            account.NickName = "PandaTV";
            ViewBag.Account = account;



            return View();
        }

        public ActionResult Test()
        {
            throw new Exception("Test ");
            return View();
        }

    }
}