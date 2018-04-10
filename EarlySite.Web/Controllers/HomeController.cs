namespace EarlySite.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Cache;
    using EarlySite.Business.Constract;
    using EarlySite.Business.IService;
    using EarlySite.Core.DDD.Service;
    using EarlySite.Model.Common;
    using EarlySite.Model.Show;

    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Account = base.CurrentAccount;

            //Result<IList<DishShare>> dishResult = ServiceObjectContainer.Get<IDishService>().SearchDishInfoByMealTime(Model.Enum.MealTime.All);
            ViewBag.DishList = null;

            return View();
        }

        public ActionResult Test()
        {
            IDishService service = ServiceObjectContainer.Get<IDishService>();

            //service.ShareDishInfo()



            return View();
        }

    }
}