namespace EarlySite.Web.Controllers
{

    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using EarlySite.Business.IService;
    using EarlySite.Core.DDD.Service;
    using EarlySite.Core.Utils;
    using EarlySite.Model.Common;
    using EarlySite.Model.Enum;
    using EarlySite.Model.Show;

    /// <summary>
    /// 发现视图控制器
    /// </summary>
    public class ExploreController : BaseController
    {
        
        /// <summary>
        /// 发现主页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            //获取用户数据
            ViewBag.Account = base.CurrentAccount;
            //获取当前定位
            Position position = base.CurrentPosition;
            //筛选附近店铺
            ShakeParam param = new ShakeParam()
            {
                Longitude = position.Longitude,
                Latitude = position.Latitude,
                NearDistance = base.SearchDistance
            };
            Result<IList<Shop>> shoplist = ServiceObjectContainer.Get<IShakeService>().ShakeNearShops(param);
            ViewBag.NearShop = shoplist.Data;



            return View();
        }
        
        /// <summary>
        /// 附近商家分布式图
        /// </summary>
        /// <param name="nearshops"></param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult NearShopPartialView(IList<Shop> nearshops)
        {
            return PartialView(nearshops);
        }

        /// <summary>
        /// 食谱详细页
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RecipeView(int recipeId)
        {
            //获取用户数据
            ViewBag.Account = base.CurrentAccount;
            //获取食谱
            Result<Recipes> recipe = ServiceObjectContainer.Get<IRecipesService>().GetRecipesById(recipeId);
            //是否是自己的食谱
            ViewBag.IsSelf = recipe.Data.Phone == CurrentAccount.Phone;
            //获取单品集合
            Result<IList<Dish>> dishlist = ServiceObjectContainer.Get<IDishService>().GetCollectDishList(recipe.Data.RecipesId);
            ViewBag.DishList = dishlist.Data;
            
            return View(recipe.Data);
        }

        /// <summary>
        /// 单品详细页
        /// </summary>
        /// <param name="dishId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DishView(int dishId)
        {
            ViewBag.Account = base.CurrentAccount;

            //获取单品信息
            Result<Dish> dishinfo = ServiceObjectContainer.Get<IDishService>().SearchDishInfoById(dishId);
            
            return View(dishinfo.Data);
        }

        /// <summary>
        /// 门店详细页
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ShopView(int shopId)
        {
            ViewBag.Account = base.CurrentAccount;

            //获取门店信息
            Result<Shop> shopinfo = ServiceObjectContainer.Get<IShopService>().GetShopInfoById(shopId);
            //获取单品集合
            //Result<IList<Dish>> dishlist = ServiceObjectContainer.Get<>().(recipe.Data.RecipesId);
            //ViewBag.DishList = dishlist.Data;

            return View(shopinfo.Data);
        }


        /// <summary>
        /// 获取门店热门单品
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ShopPopDish(int shopId)
        {
            Result<IList<Dish>> dishlist = ServiceObjectContainer.Get<IShakeService>().ShakePopDishForShop(shopId);

            return Json(dishlist);
        }



    }
}