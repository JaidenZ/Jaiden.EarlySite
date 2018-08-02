namespace EarlySite.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
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

            //创建分页参数
            PageSearchParam param = new PageSearchParam();
            param.PageIndex = 1;
            param.PageNumer = 4;

            //获取推荐单品信息
            MealTime meal = GetMealTimeForNow();
            Result<PageList<Dish>> dishresult = ServiceObjectContainer.Get<IDishService>().SearchDishInfoByMealTime(meal, param);
            ViewBag.DishList = dishresult.Data;
            
            //获取推荐食谱信息
            Result<PageList<Recipes>> recipesresult = ServiceObjectContainer.Get<IRecipesService>().GetPageRecipes(param);
            ViewBag.RecipesList = recipesresult.Data;

            ViewBag.Meal = meal;

            return View();
        }

        /// <summary>
        /// 今日食分布式图
        /// </summary>
        /// <returns></returns>
        public PartialViewResult TodayDishPartialView()
        {
            ViewBag.Meal = GetMealTimeForNow();
            return PartialView();
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
        /// 食谱推荐分布视图
        /// </summary>
        /// <param name="pagelist"></param>
        /// <returns></returns>
        public PartialViewResult PushRecipesInfoPartialView(PageList<Recipes> pagelist)
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
            MealTime meal = MealTime.所有时间段;
            if (now.Hour >= 5 && now.Hour < 11)
            {
                meal = MealTime.早餐;
            }
            if (now.Hour >= 11 && now.Hour < 14)
            {
                meal = MealTime.午餐;
            }
            if (now.Hour >= 14 && now.Hour < 17)
            {
                meal = MealTime.下午茶;
            }
            if (now.Hour >= 17 && now.Hour < 21)
            {
                meal = MealTime.晚餐;
            }
            if (now.Hour >= 21 || now.Hour < 5)
            {
                meal = MealTime.夜宵;
            }
            return meal;
        }

        /// <summary>
        /// 摇一摇获取今日的菜谱
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ShakeTodayDish(ShakeParam param)
        {
            param.NearDistance = 1000;
            //筛选附近店铺
            Result<IList<Shop>> dishresult = ServiceObjectContainer.Get<IShakeService>().ShakeNearShops(param);
            
            return Json(dishresult);
        }


        /// <summary>
        /// 摇一摇附近商铺
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ShakeNearShop(ShakeParam param)
        {
            param.NearDistance = base.SearchDistance;
            //存放当前定位
            HttpContext.Session["Position"] = string.Format("{0},{1}", param.Longitude, param.Latitude);

            //筛选附近店铺
            Result<IList<Shop>> shoplist = ServiceObjectContainer.Get<IShakeService>().ShakeNearShops(param);
            
            return Json(shoplist, JsonRequestBehavior.AllowGet);
        }

    }
}