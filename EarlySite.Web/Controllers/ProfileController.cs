namespace EarlySite.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class ProfileController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {



            ViewBag.Account = base.CurrentAccount;
            ViewBag.Titile = "EarlySite | Profile";

            return View();
        }
    }
}