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
        public ActionResult Index()
        {

            ViewBag.Account = base.CurrentAccount;
            ViewBag.Titile = "EarlySite | Profile";

            //获取用户的食谱列表
            Result<IList<Recipes>> recipesresult = ServiceObjectContainer.Get<IRecipesService>().GetRecipesByPhone(base.CurrentAccount.Phone);
            ViewBag.Recipes = recipesresult.Data;

            return View();
        }

        /// <summary>
        /// 菜谱分布视图
        /// </summary>
        /// <param name="recipes"></param>
        /// <returns></returns>
        public PartialViewResult RecipesPatialView(IList<Recipes> recipes)
        {
            return PartialView(recipes);
        }


        [HttpGet]
        public ActionResult Setting()
        {
            return View();
        }

    }
}