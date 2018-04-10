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
            IDishService dish = ServiceObjectContainer.Get<IDishService>();
            IShopService shop = ServiceObjectContainer.Get<IShopService>();
            IRecipesService recipes = ServiceObjectContainer.Get<IRecipesService>();

            Shop shopmodel = new Shop();
            shopmodel.ShopId = Convert.ToInt32(string.Format("10{0}",System.DateTime.Now.ToString("yyMMdd")));
            shopmodel.UpdateDate = DateTime.Now;
            shopmodel.ShopName = "成都人民南路四段门店";
            shopmodel.Description = "测试门店";
            shopmodel.Longitude = 104.073773;
            shopmodel.Latitude = 30.638901;

            Recipes recipesmodel = new Recipes();
            recipesmodel.RecipesId = Convert.ToInt32(string.Format("20{0}", System.DateTime.Now.ToString("yyMMdd")));
            recipesmodel.Name = "喜欢的食谱";
            recipesmodel.IsPrivate = false;
            recipesmodel.Phone = 18502850589;
            recipesmodel.Description = "测试食谱";
            recipesmodel.UpdateDate = DateTime.Now;


            Dish dishmodel = new Dish();
            dishmodel.DIshId = Convert.ToInt32(string.Format("30{0}", System.DateTime.Now.ToString("yyMMdd")));
            dishmodel.Name = "葱白肉丝";
            dishmodel.Type = Model.Enum.DishType.川菜;
            dishmodel.MealTime = Model.Enum.MealTime.All;
            dishmodel.UpdateDate = DateTime.Now;
            dishmodel.Image = "";
            dishmodel.Description = "测试单品";
            dishmodel.ShopId = shopmodel.ShopId;
            dishmodel.ShopName = shopmodel.ShopName;


            return View();
        }

    }
}