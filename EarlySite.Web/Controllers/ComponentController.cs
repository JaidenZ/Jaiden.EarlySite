namespace EarlySite.Web.Controllers
{
    using System.Web.Mvc;
    using System.Collections.Generic;
    using EarlySite.Model.Common;
    using EarlySite.Model.Show;
    using EarlySite.Core.DDD.Service;
    using EarlySite.Business.IService;
    using EarlySite.Model.Enum;
    using System;

    /// <summary>
    /// 组件控制器
    /// </summary>
    public class ComponentController : BaseController
    {

        /// <summary>
        /// 生成单品分享组件界面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GenerateDishShareView()
        {
            //获取当前用户的食谱集
            ViewBag.Recipes = ServiceObjectContainer.Get<IRecipesService>().GetRecipesByPhone(CurrentAccount.Phone).Data;
            ViewBag.Meal = (int)MealTime.所有时间段;
            return View();
        }

        /// <summary>
        /// 生成食谱组件界面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GenerateRecipesView()
        {

            return View();
        }

        /// <summary>
        /// 生成商家信息组件界面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GenerateStoreView()
        {
            return View();
        }


        /// <summary>
        /// 模糊搜索门店信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SearchKeyforShopInfo(string key)
        {
            Result<PageList<Shop>> result = ServiceObjectContainer.Get<IShopService>().SearchShopInfoByName(key, new PageSearchParam() { PageIndex = 1, PageNumer = 5 });   
            return Json(result);
        }
        
        /// <summary>
        /// 分享单品食物信息
        /// </summary>
        /// <param name="dish"></param>
        /// <param name="shopid"></param>
        /// <param name="recipeid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DishShare(Dish dish,int shopid,int recipeid)
        {
            IDishService dishservice = ServiceObjectContainer.Get<IDishService>();
            IRecipesService recipesservice = ServiceObjectContainer.Get<IRecipesService>();
            IShopService shopservice = ServiceObjectContainer.Get<IShopService>();
            Shop shopselect = shopservice.GetShopInfoById(shopid).Data;
            Recipes recipesselect = recipesservice.GetRecipesById(recipeid).Data;

            dish.DIshId = Generation.GenerationId();
            dish.UpdateDate = DateTime.Now;
            dish.ShopId = shopselect.ShopId;
            dish.ShopName = shopselect.ShopName;
            if (!string.IsNullOrEmpty(dish.Image))
            {
                int substringindex = dish.Image.LastIndexOf(',') + 1;
                dish.Image = dish.Image.Substring(substringindex, dish.Image.Length - substringindex);
            }



            DishShare share = new DishShare();
            share.DishInfo = dish;
            share.RecipesInfo = recipesselect;
            share.ShopInfo = shopselect;

            return Json(dishservice.ShareDishInfo(share));
        }

        /// <summary>
        /// 创建食谱信息
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerateRecipe(Recipes recipe)
        {
            IRecipesService recipesservice = ServiceObjectContainer.Get<IRecipesService>();
            recipe.RecipesId = Generation.GenerationId();
            recipe.UpdateDate = DateTime.Now;
            recipe.Phone = CurrentAccount.Phone;

            Result result = recipesservice.CreatRecipes(recipe);

            return Json(result);
        }
    }
}