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
            
            return View();
        }

        /// <summary>
        /// 单品推荐分布视图
        /// </summary>
        /// <returns></returns>
        public PartialViewResult PushDishInfoPartialView()
        {


            return PartialView();

        }



        
    }
}