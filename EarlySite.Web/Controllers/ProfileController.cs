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
            ViewBag.Account = ServiceObjectContainer.Get<IAccountService>().GetAccountInfo(phone).Data;
            
            //获取用户的食谱列表
            Result<IList<Recipes>> recipesresult = ServiceObjectContainer.Get<IRecipesService>().GetRecipesByPhone(phone);
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
        public ActionResult Setting(long phone)
        {
            //获取用户数据
            ViewBag.Account = ServiceObjectContainer.Get<IAccountService>().GetAccountInfo(phone).Data;
            return View();
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