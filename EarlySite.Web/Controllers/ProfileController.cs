namespace EarlySite.Web.Controllers
{
    using EarlySite.Business.IService;
    using EarlySite.Core.DDD.Service;
    using EarlySite.Model.Common;
    using EarlySite.Model.Show;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class ProfileController : BaseController
    {
        [HttpGet]
        public ActionResult Index(long phone)
        {
            //获取用户数据
            ViewBag.Account = base.CurrentAccount;

            Account viewaccount = ServiceObjectContainer.Get<IAccountService>().GetAccountInfo(phone).Data;
            //获取用户的食谱列表
            Result<IList<Recipes>> recipesresult = ServiceObjectContainer.Get<IRecipesService>().GetRecipesByPhone(phone);
            ViewBag.Recipes = recipesresult.Data;
            //获取用户分享的单品列表
            Result<IList<Dish>> dishresult = ServiceObjectContainer.Get<IDishService>().GetShareDishList(phone);
            ViewBag.Dishs = dishresult.Data;

            //获取用户收藏的食谱
            //Result<IList<Recipes>> recipesresult = ServiceObjectContainer.Get<IRecipesService>().GetRecipesByPhone(phone);
            ViewBag.FavoriteRecipes = recipesresult.Data;
            //获取用户收藏的单品
            //Result<IList<Dish>> dishresult = ServiceObjectContainer.Get<IDishService>().GetShareDishList(phone);
            ViewBag.FavoriteDishs = dishresult.Data;

            return View(viewaccount);
        }

        /// <summary>
        /// 菜谱分布视图
        /// </summary>
        /// <param name="recipes"></param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult RecipesPatialView(IList<Recipes> recipes)
        {
            return PartialView(recipes);
        }

        /// <summary>
        /// 单品分布视图
        /// </summary>
        /// <param name="dishs"></param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult DishPatialView(IList<Dish> dishs)
        {
            return PartialView(dishs);
        }

        /// <summary>
        /// 收藏单品分布视图
        /// </summary>
        /// <param name="favoritedish"></param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult FavoriteDishPatialView(IList<Dish> favoritedish)
        {
            return PartialView(favoritedish);
        }

        /// <summary>
        /// 收藏食谱分布视图
        /// </summary>
        /// <param name="favoritedish"></param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult FavoriteRecipePatialView(IList<Recipes> favoriteRecipe)
        {
            return PartialView(favoriteRecipe);
        }

        [HttpGet]
        public ActionResult Setting()
        {
            ViewBag.Account = CurrentAccount;
            return View(base.CurrentAccount);
        }
        
        /// <summary>
        /// 改变账户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ChangeAccountInfo(Account account)
        {
            Result result = ServiceObjectContainer.Get<IAccountService>().UpdateAccountInfo(account);
            return Json(result);
        }



    }
}