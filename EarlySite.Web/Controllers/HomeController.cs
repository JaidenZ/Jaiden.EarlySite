using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EarlySite.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {


            return View();
        }

        public ActionResult Test()
        {
            throw new Exception("Test ");
            return View();
        }

    }
}