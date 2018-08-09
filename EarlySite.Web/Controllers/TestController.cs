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
    using System.Linq;

    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GenerationId()
        {
            return Json(Generation.GenerationId());
        }


        public JsonResult ShareDish(int shopId,int recipesId)
        {
            IDishService dish = ServiceObjectContainer.Get<IDishService>();
            IRecipesService recipes = ServiceObjectContainer.Get<IRecipesService>();
            IShopService shop = ServiceObjectContainer.Get<IShopService>();
            Shop shopselect = shop.GetShopInfoById(shopId).Data;
            Recipes recipesselect = recipes.GetRecipesById(recipesId).Data;

            Dish dishmodel = new Dish();
            dishmodel.DIshId = Generation.GenerationId();
            dishmodel.Name = "黑椒牛柳";
            dishmodel.Type = Model.Enum.DishType.川菜.GetHashCode();
            dishmodel.MealTime = Model.Enum.MealTime.午餐.GetHashCode();
            dishmodel.UpdateDate = DateTime.Now;
            dishmodel.Image = "";
            dishmodel.Description = "测试单品";
            dishmodel.ShopId = shopselect.ShopId;
            dishmodel.ShopName = shopselect.ShopName;

            DishShare share = new DishShare();
            share.DishInfo = dishmodel;
            share.RecipesInfo = recipesselect;
            share.ShopInfo = shopselect;

            return Json(dish.ShareDishInfo(share));
        }

        public JsonResult AddStore()
        {

            IShopService shop = ServiceObjectContainer.Get<IShopService>();
            Shop shopmodel = new Shop();
            shopmodel.ShopId = Generation.GenerationId();
            shopmodel.UpdateDate = DateTime.Now;
            shopmodel.ShopName = "成都人民南路四段门店";
            shopmodel.Description = "测试门店";
            shopmodel.Longitude = 104.073773;
            shopmodel.Latitude = 30.638901;

            return Json(shop.CreatShopInfo(shopmodel));

        }

        public JsonResult AddRecipes()
        {
            IRecipesService recipes = ServiceObjectContainer.Get<IRecipesService>();
            

            Recipes recipesmodel = new Recipes();
            recipesmodel.RecipesId = Generation.GenerationId();
            recipesmodel.Name = "清淡的";
            recipesmodel.IsPrivate = false;
            recipesmodel.Phone = 18502850589;
            recipesmodel.Description = "测试食谱";
            recipesmodel.UpdateDate = DateTime.Now;

            return Json(recipes.CreatRecipes(recipesmodel));
        }

        public JsonResult GetShopPageList(int pageindex)
        {
            IShopService shops = ServiceObjectContainer.Get<IShopService>();

            PageSearchParam param = new PageSearchParam();
            param.PageNumer = 3;
            param.SearchType = 0;
            param.PageIndex = pageindex;

            
            return Json(shops.GetShopPageList(param));
        }

    }
}