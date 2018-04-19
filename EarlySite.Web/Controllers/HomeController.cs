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
    using EarlySite.Model.Enum;
    using EarlySite.Model.Show;

    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Account = base.CurrentAccount;

            //根据时间获取单品信息
            MealTime meal = GetMealTimeForNow();
            PageSearchParam param = new PageSearchParam();
            param.PageIndex = 1;
            param.PageNumer = 6;
            Result<PageList<Dish>> dshresult = ServiceObjectContainer.Get<IDishService>().SearchDishInfoByMealTime(meal, param);
            ViewBag.DishList = dshresult.Data;

            return View();
        }

        /// <summary>
        /// 单品推荐分布视图
        /// </summary>
        /// <returns></returns>
        public PartialViewResult PushDishInfoPartialView(PageList<Dish> pagelist)
        {
            return PartialView(pagelist);
        }

        /// <summary>
        /// 根据时间获取用餐时间类型
        /// </summary>
        /// <returns></returns>
        private MealTime GetMealTimeForNow()
        {
            DateTime now = DateTime.Now;
            MealTime meal = MealTime.All;
            if (now.Hour >= 5 && now.Hour < 11)
            {
                meal = MealTime.BreakFast;
            }
            if (now.Hour >= 11 && now.Hour < 14)
            {
                meal = MealTime.Lunch;
            }
            if (now.Hour >= 14 && now.Hour < 17)
            {
                meal = MealTime.TeaTime;
            }
            if (now.Hour >= 17 && now.Hour < 21)
            {
                meal = MealTime.Dinner;
            }
            if (now.Hour >= 21 || now.Hour < 5)
            {
                meal = MealTime.NightSnack;
            }
            return meal;
        }

    }
}