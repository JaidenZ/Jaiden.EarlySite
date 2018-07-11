namespace EarlySite.Web.Controllers
{
    using System.Web.Mvc;
    using System.Collections.Generic;
    using EarlySite.Model.Common;
    using EarlySite.Model.Show;
    using EarlySite.Core.DDD.Service;
    using EarlySite.Business.IService;

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

            ViewBag.Meal = 1;
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



    }
}