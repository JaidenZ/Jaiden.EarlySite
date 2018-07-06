namespace EarlySite.Web.Controllers
{

    using System;
    using System.Web.Mvc;
    using EarlySite.Business.IService;
    using EarlySite.Core.DDD.Service;
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
            return View();
        }
    }
}