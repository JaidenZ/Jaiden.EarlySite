namespace EarlySite.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using EarlySite.SModel;

    public class ProfileController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            Account account = new Account();
            account.NickName = "PandaTV";
            ViewBag.Account = account;

            ViewBag.Titile = "EarlySite | Profile";

            return View();
        }
    }
}