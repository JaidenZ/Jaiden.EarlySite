using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EarlySite.Web.Controllers
{
    public class ProfileController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Titile = "EarlySite | Profile";

            return View();
        }
    }
}